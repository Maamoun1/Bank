using BusinessLayer.DTOs.Accounts;
using BusinessLayer.DTOs.People;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBank.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


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
        public async Task<IActionResult> GetAccountByID(int accountId)
        {

            try
            {

                if (accountId < 0)
                {
                    return BadRequest($"Not Accepted ID: {accountId}");
                }

                var account = await _accountService.GetAsync(a => a.AccountId == accountId, "Application,Client,CreatedByUser");

                if (account != null)
                {
                    ReteriveAccountDto reteriveAccountDto = new ReteriveAccountDto()
                    {
                        AccountId = accountId,
                        PinCode = account.PinCode,
                        IssueReason = account.IssueReason,
                        Balance = account.Balance,
                        IsActive = account.IsActive,

                    };
                    return Ok(new { success = true, data = reteriveAccountDto });
                }

                else
                {
                    return NotFound($"account with ID {accountId} Not Found.");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving an Account." });
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


        [HttpPut(Name = "UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(string pinCode, UpdatePassword dto)
        {

            if (pinCode =="")
                return BadRequest($"Not Accepted ID: {pinCode}");

            if (!_accountService.IsAccountExist(pinCode))
                NotFound("Account is not Found exist..");

            await _accountService.UpdatePassword(pinCode, dto);
            await _accountService.SaveChanges();

            return Ok(dto);
        }



    }
}
