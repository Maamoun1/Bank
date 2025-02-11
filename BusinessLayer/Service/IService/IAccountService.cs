using BusinessLayer.DTOs.Accounts;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.IService
{
    public interface IAccountService:IGenericeService<TbAccount>
    {
        Task DecativeAccount(int accountId);
        Task AddAccount(CreateAccountDto dto);
        Task UpdatePassword(string pinCode,UpdatePassword dto);    
        bool IsAccountExist(string pinCode);
        Task<string> GetPassword(string pinCode);

    }
}
