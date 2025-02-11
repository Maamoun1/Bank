using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AccountTypeService : IAccountTypeService
    {

        private readonly IAccountsTypesRepository _accountsTypesRepository;

        public AccountTypeService(IAccountsTypesRepository accountsTypesRepository)
        {
            _accountsTypesRepository = accountsTypesRepository;
        }

        public Task<IEnumerable> GetAccountTypeAsync()
        {
            return _accountsTypesRepository.GetAllAsync();
        }
    }


}
