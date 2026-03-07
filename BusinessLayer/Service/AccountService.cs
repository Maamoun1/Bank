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
        
        private readonly ICacheService _cache;
        public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork,ICacheService cahche) : base(accountRepository)
        {

            _unitOfWork = unitOfWork;
            _cache = cahche;
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

        public  async Task <bool> IsAccountExistAsync(string accountNumber)
        {
            return await _unitOfWork.Account.IsAccountExist(accountNumber);
        }

        public async Task DepositeAsync(string accountNumber, double balance)
        {

            if (balance < 100 || !await IsAccountExistAsync(accountNumber))
               throw new Exception("Balance must be increase 100 or an account is not exist");
            
              await  _unitOfWork.Account.DepositeAsync(accountNumber, balance);
              await _unitOfWork.SaveAsync();


            var cacheKey = CacheKeys.Balance(accountNumber);
            await _cache.RemoveAsync(cacheKey);
        }

        public async Task WithdrawAsync(string accountNumber, double balance)
        {
            if (balance < 100 || !await IsAccountExistAsync(accountNumber))
                throw new Exception("Balance must be increase 100 or account does not exist");

            var lockKey = $"lock:account:{accountNumber}";
            var lockValue = Guid.NewGuid().ToString();

            try
            {
                // Acquire Lock
                await _cache.SetAsync(lockKey, lockValue, TimeSpan.FromSeconds(5));

                // Withdraw from database
                await _unitOfWork.Account.WithdrawAsync(accountNumber, balance);

                // Save changes
                await _unitOfWork.SaveAsync();

                // Invalidate cache
                var cacheKey = CacheKeys.Balance(accountNumber);
                await _cache.RemoveAsync(cacheKey);
            }
            finally
            {
                // Release Lock
                await _cache.RemoveAsync(lockKey);
            }
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

        public async Task<double> GetBalanceAsync(string accountNumber)
        {

            var cachekey = CacheKeys.Balance(accountNumber);

            //cacahe redis
            var cachedBalance = await _cache.GetAsync<double?>(cachekey);

            if (cachedBalance !=  null)
                return cachedBalance.Value;

            //Fallback to database
            var balance = await _unitOfWork.Account.GetBalanceAsync(accountNumber);


            if (balance is null)
                throw new Exception("Account is not found");


            await _cache.SetAsync(cachekey, balance.Value,TimeSpan.FromMinutes(5));



            return balance.Value;

        }
    }
}
