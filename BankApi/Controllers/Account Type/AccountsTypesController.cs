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

     



    }
}
