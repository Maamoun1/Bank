using BusinessLayer.DTOs.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBank.Controllers.Account
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var accounts = await _accountService.GetAllAsync();
                return Ok(new { success = true, data = accounts });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving accounts." });
            }
        }

        [HttpGet("GetByAccountNumber/{accountNumber}", Name = "GetAccountByNumber")]
        public async Task<IActionResult> GetAccountByNumber(string accountNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                    return BadRequest(new { success = false, message = "Account number is required." });

                var account = await _accountService.GetAsync(
                    a => a.AccountNumber == accountNumber,
                    "Application,Client,CreatedByUser,Client.Person");

                if (account == null)
                    return NotFound(new { success = false, message = $"Account '{accountNumber}' not found." });

                var dto = new ReteriveAccountDto
                {
                    AccountNumber = account.AccountNumber,
                    ClientName = account.Client.Person.FirstName + " " + account.Client.Person.LastName,
                    IssueReason = account.IssueReason,
                    Balance = account.Balance,
                    IsActive = account.IsActive,
                };

                return Ok(new { success = true, data = dto });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving the account." });
            }
        }


        [HttpGet("GetBalance/{accountNumber}", Name = "GetBalance")]
        public async Task<IActionResult> GetBalance(string accountNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                    return BadRequest(new { success = false, message = "Account number is required." });

                var balance = await _accountService.GetBalanceAsync(accountNumber);
                return Ok(new { success = true, data = balance });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving the balance." });
            }
        }

        [HttpPost(Name = "AddAccount")]
        public async Task<IActionResult> AddAccount([FromBody] CreateAccountDto dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid account data." });

            await _accountService.AddAccount(dto);
            await _accountService.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Account created successfully. The PIN has been issued via the secure channel."
            });
        }

   
        [HttpPost("VerifyPin", Name = "VerifyPin")]
        public async Task<IActionResult> VerifyPin([FromBody] VerifyPinDto dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid request." });

            var isValid = await _accountService.VerifyPinAsync(dto.AccountNumber, dto.Pin);

            if (!isValid)
                return Unauthorized(new { success = false, message = "Invalid PIN." });

            return Ok(new { success = true, message = "PIN verified." });
        }


        [HttpPut("ChangePin/{accountNumber}", Name = "ChangePin")]
        public async Task<IActionResult> ChangePin(string accountNumber, [FromBody] UpdatePinDto dto)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { success = false, message = "Account number is required." });

            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid request body." });

            try
            {
                await _accountService.UpdatePinAsync(accountNumber, dto);
                await _accountService.SaveChanges();

                return Ok(new { success = true, message = "PIN changed successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while changing the PIN." });
            }
        }


        [HttpPut("Deposit")]
        public async Task<IActionResult> Deposit(string accountNumber, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { success = false, message = "Account number is required." });

            try
            {
                await _accountService.DepositeAsync(accountNumber, balance);
                return Ok(new { success = true, message = "Deposit successful." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred during deposit." });
            }
        }

    
        [HttpPut("Withdraw")]
        public async Task<IActionResult> Withdraw(string accountNumber, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return BadRequest(new { success = false, message = "Account number is required." });

            try
            {
                await _accountService.WithdrawAsync(accountNumber, balance);
                return Ok(new { success = true, message = "Withdrawal successful." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("currently being processed"))
            {
                // Lock contention — signal the client to retry
                return Conflict(new { success = false, message = ex.Message, retryAfterMs = 200 });
            }
            catch (InvalidOperationException ex)
            {
                // Insufficient funds or inactive account
                return UnprocessableEntity(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred during withdrawal." });
            }
        }

        // -------------------------------------------------------------------
        // TRANSFER
        //
        // Calls TransferAsync — dual Redis locks, deterministic order,
        // balance check inside the lock, SQL transaction for atomicity.
        //
        // HTTP status mapping:
        //   ArgumentException        → 400 Bad Request
        //   KeyNotFoundException     → 404 Not Found
        //   InvalidOperationException (lock contention) → 409 Conflict
        //   InvalidOperationException (insufficient funds) → 422 Unprocessable
        //   Any other exception      → 500 Internal Server Error
        // -------------------------------------------------------------------

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDto dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid request body." });

            try
            {
                await _accountService.TransferAsync(dto.SenderId, dto.ReceiverId, dto.Amount);
                return Ok(new { success = true, message = "Transfer succeeded." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("currently being processed"))
            {
                // One or both accounts are locked by another concurrent request.
                // 409 Conflict — the client should retry after a short delay.
                return Conflict(new
                {
                    success = false,
                    message = ex.Message,
                    retryAfterMs = 300
                });
            }
            catch (InvalidOperationException ex)
            {
                // Insufficient funds or same-account transfer.
                // 422 Unprocessable — valid request, failed business rule.
                return UnprocessableEntity(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred during transfer." });
            }
        }
    }
}