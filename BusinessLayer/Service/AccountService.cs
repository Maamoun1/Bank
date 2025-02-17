using BusinessLayer.DTOs.Accounts;
using BusinessLayer.Global_Class;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository;
using DataAccessLayer.Respository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AccountService : GenericService<TbAccount>, IAccountService
    {

        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork) : base(accountRepository)
        {

            _unitOfWork = unitOfWork;
        }

        public Task DecativeAccount(int accountId)
        {
            throw new NotImplementedException();
        }


        public  async Task AddAccount(CreateAccountDto dto)
        {

            var newAccount = new TbAccount()
            {
                ApplicationId = dto.ApplicationId,
                ClientId = dto.ClientId,
                AccountNumber = Util.GenerateAccountNumber(5),
                Password = Util.Encrypt(Util.password, Util.GetPublicKey()),
                IssueDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddYears(3),
                IssueReason = dto.IssueReason,
                Balance = 0,
                IsActive = true,
                CreatedByUserId = dto.CreatedByUserId,
            };
            await _unitOfWork.Account.AddAsync(newAccount);

        }

        public async Task UpdatePassword(string accountNumber, UpdatePassword dto)
        {

            var accountFromDb = await _unitOfWork.Account.GetAsync(p => p.AccountNumber == accountNumber);

            if (accountFromDb != null)
            {
                
                accountFromDb.Password = Util.Encrypt(dto.Password, Util.GetPublicKey());
                _unitOfWork.Account.Update(accountFromDb);
            }
        }

        public async Task<string> GetPassword(string accountNumber)
        {
            
            var accountFromDb = await _unitOfWork.Account.GetAsync(p => p.AccountNumber == accountNumber, null, true);

            if (accountFromDb != null)
            {
                return Util.Decrypt(accountFromDb.Password, Util.GetPrivateKey());
            }
            else
                return $"Not account with [{accountNumber}]";
        }

        public bool IsAccountExist(string accountNumber)
        {
            return _unitOfWork.Account.IsAccountExist(accountNumber);
        }

        public void Deposite(string accountNumber, double balance)
        {

            if (balance < 100 || !IsAccountExist(accountNumber))
                Console.WriteLine("Balance must be increase 100 or an account is not exist");
            else
                _unitOfWork.Account.Deposite(accountNumber, balance); 
        }

        public void Withdraw(string accountNumber, double balance)
        {
            if (balance < 100 || !IsAccountExist(accountNumber))
                Console.WriteLine("Balance must be increase 100 or an account is not exist");
            else
                _unitOfWork.Account.Withdraw(accountNumber, balance);
        }

        public double  GetBalance(string accountNumber)
        {
            return _unitOfWork.Account.GetBalance(accountNumber);
        }

        public async Task<bool> TransferAmountAsync(string senderId, string receiverId, double amount)
        {

            if (amount < 0)
            {
                Console.WriteLine("Amount must be increase 0");
                return false;
            }

            return await _unitOfWork.Account.TransferAmountAsync(senderId, receiverId, amount);
        }
    }
}
