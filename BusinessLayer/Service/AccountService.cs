using BusinessLayer.DTOs.Accounts;
using BusinessLayer.Global_Class;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository;
using DataAccessLayer.Respository.IRepository;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
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
                PinCode = Util.GenerateRandomString(5),
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

        public async Task UpdatePassword(string pinCode,UpdatePassword dto)
        {

            var accountFromDb = await _unitOfWork.Account.GetAsync(p => p.PinCode == pinCode);

            if (accountFromDb != null)
            {

                accountFromDb.Password = Encrypt(dto.Password, Util.GetPublicKey());
                _unitOfWork.Account.Update(accountFromDb);
            }
        }

        public async Task<string> GetPassword(string pinCode)
        {

            var accountFromDb = await _unitOfWork.Account.GetAsync(p => p.PinCode == pinCode, null, true);

            return Decrypt(accountFromDb.Password, Util.GetPrivateKey());

        }

        public bool IsAccountExist(string pinCode)
        {
            return _unitOfWork.Account.IsAccountExist(pinCode);
        }

    }
}
