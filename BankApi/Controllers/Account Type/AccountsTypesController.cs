using BusinessLayer.DTOs.Account_Type;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBank.Controllers.Account_Type
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsTypesController : ControllerBase
    {

        private readonly IAccountTypeService _accountType;

        public AccountsTypesController(IAccountTypeService accountsTypes)
        {
            _accountType = accountsTypes;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var lastAccountsTypes = await _accountType.GetAccountTypeAsync();

                return Ok(new { success = true, data = lastAccountsTypes });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving Accounts Types." });
            }
        }



    }
}
