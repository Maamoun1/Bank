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
        bool IsAccountExist(string accountNumber);
        Task<string> GetPassword(string accountNumber);
        void Deposite(string accountNumber, double balance);
        void Withdraw(string accountNumber, double balance);
        double GetBalance(string pinCode);

        Task<bool> TransferAmountAsync(string senderId, string receiverId, double amount);
    }
    

}
