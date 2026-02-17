using BusinessLayer.DTOs.Accounts;
using BusinessLayer.DTOs.People;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

                var lstAccounts =await _accountService.GetAllAsync();

                return Ok(new { success = true, data = lstAccounts });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving an Account." });
            }
        }


        [HttpGet("GetByID/{accountId}", Name = "GetAccountByID")]
        public async Task<IActionResult> GetAccountByID(string accountNumber)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    return BadRequest($"Not Accepted ID: {accountNumber}");
                }

                var account = await _accountService.GetAsync(a => a.AccountNumber == accountNumber, "Application,Client,CreatedByUser,Client.Person");

                if (account != null)
                {
                    ReteriveAccountDto reteriveAccountDto = new ReteriveAccountDto()
                    {
                        AccountNumber = accountNumber,
                        ClientName=account.Client.Person.FirstName+' '+ account.Client.Person.LastName,
                        IssueReason = account.IssueReason,
                        Balance = account.Balance,
                        IsActive = account.IsActive,

                    };
                    return Ok(new { success = true, data = reteriveAccountDto });
                }

                else
                {
                    return NotFound($"account with ID {accountNumber} Not Found.");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving an Account." });
            }

        }


        [HttpGet("GetBalance/{accountNumber}", Name = "GetBalance")]
        public async Task<IActionResult> GeBalance(string accountNumber)
        {

            try
            {

                if (accountNumber == "")
                {
                    return BadRequest($"Not Accepted ID: {accountNumber}");
                }

                double balance= _accountService.GetBalance(accountNumber);

                return Ok(new { success = true, data = balance });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving a balance." });
            }

        }


        [HttpGet("GetPassword/{pinCode}", Name = "GetPassword")]
        public async Task<IActionResult> GetPassword(string pinCode)
        {

            try
            {

                if (pinCode =="")
                {
                    return BadRequest($"Not Accepted ID: {pinCode}");
                }

                var accountPassword = await _accountService.GetPassword(pinCode);

                return Ok(new { success = true, data = accountPassword });

                //else
                //{
                //    return NotFound($"account with ID {pinCode} Not Found.");
                //}
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving an Account." });
            }

        }


        [HttpPost(Name = "AddAccount")]
        public async Task<IActionResult> AddAccount(CreateAccountDto dto)
        {

            if (dto == null)
            {
                return BadRequest("Invalid Person data.");
            }

            await _accountService.AddAccount(dto);
            await _accountService.SaveChanges();
            return Ok(dto);
        }


        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(string AccountNumber, UpdatePassword dto)
        {

            if (string.IsNullOrWhiteSpace(AccountNumber))
                return BadRequest($"Not Accepted ID: {AccountNumber}");

            if (!_accountService.IsAccountExist(AccountNumber))
                NotFound("Account is not Found exist..");

            await _accountService.UpdatePassword(AccountNumber, dto);
            await _accountService.SaveChanges();

            return Ok(dto);
        }


        [HttpPut("Deposite")]
        public async Task<IActionResult>Deposite(string accountNumber, double balance)
        {

            if (string.IsNullOrWhiteSpace(accountNumber))
              return  BadRequest($"Not Accepted {accountNumber}");

            if (!_accountService.IsAccountExist(accountNumber))
              return  NotFound("Account is not exist");

             _accountService.Deposite(accountNumber, balance);
             await _accountService.SaveChanges();

            return Ok("Success deposite");
        }

         [HttpPut("Withdraw")]
        public async Task<IActionResult>Withdraw(string accountNumber, double balance)
        {

            if (string.IsNullOrWhiteSpace(accountNumber))
              return  BadRequest($"Not Accepted {accountNumber}");

            if (!_accountService.IsAccountExist(accountNumber))
              return  NotFound("Account is not exist");

             _accountService.Withdraw(accountNumber, balance);
             await _accountService.SaveChanges();

            return Ok("Ahmed adel is a super hero");
        }


        [HttpPut("Transfer")]
        public async Task<IActionResult> TransferAmount(string senderId,string receiverId, double balance)
        {

            if (await _accountService.TransferAmountAsync(senderId, receiverId, balance))
            {
                await _accountService.SaveChanges();
                return Ok("Transfer Succeeded");
            }

            else
                return NotFound("Transfer Failed..");
        }




    }
}
