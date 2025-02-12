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

        static string Encrypt(string plainText, string publicKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);


                    byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
                    return Convert.ToBase64String(encryptedData);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Encryption error: {ex.Message}");
                throw; // Rethrow the exception to be caught in the Main method
            }
        }

        static string Decrypt(string cipherText, string privateKey)
        {
            try
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);


                    byte[] encryptedData = Convert.FromBase64String(cipherText);
                    byte[] decryptedData = rsa.Decrypt(encryptedData, false);


                    return Encoding.UTF8.GetString(decryptedData);
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Decryption error: {ex.Message}");
                throw; // Rethrow the exception to be caught in the Main method
            }
        }

        public  async Task AddAccount(CreateAccountDto dto)
        {

            var newAccount = new TbAccount()
            {
                ApplicationId = dto.ApplicationId,
                ClientId = dto.ClientId,
                AccountNumber = Util.GenerateAccountNumber(5),
                Password = Encrypt(Util.password, Util.GetPublicKey()),
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

                accountFromDb.Password = Encrypt(dto.Password, Util.GetPublicKey());
                _unitOfWork.Account.Update(accountFromDb);
            }
        }

        public async Task<string> GetPassword(string accountNumber)
        {
            
            var accountFromDb = await _unitOfWork.Account.GetAsync(p => p.AccountNumber == accountNumber, null, true);

            if (accountFromDb != null)
            {
                return Decrypt(accountFromDb.Password, Util.GetPrivateKey());
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
