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
        Task UpdatePassword(string accountNumber, UpdatePassword dto);    
       Task <bool> IsAccountExistAsync(string accountNumber);
        Task<string> GetPassword(string accountNumber);
        Task DepositeAsync(string accountNumber, double balance);
        Task WithdrawAsync(string accountNumber, double balance);
        Task<double> GetBalanceAsync(string accountNumber);

        Task<bool> TransferAmountAsync(string senderId, string receiverId, double amount);
    }
    

}
