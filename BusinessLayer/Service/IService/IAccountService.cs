using BusinessLayer.DTOs.Accounts;
using DataAccessLayer.Entities;

namespace BusinessLayer.Service.IService
{
    public interface IAccountService : IGenericeService<TbAccount>
    {
        Task DecativeAccount(int accountId);

        Task AddAccount(CreateAccountDto dto);

        Task<bool> VerifyPinAsync(string accountNumber, string submittedPin);

        Task UpdatePinAsync(string accountNumber, UpdatePinDto dto);

        Task<bool> IsAccountExistAsync(string accountNumber);

        Task DepositeAsync(string accountNumber, double balance);

        Task WithdrawAsync(string accountNumber, double balance);

        Task<double> GetBalanceAsync(string accountNumber);

        Task<bool> TransferAmountAsync(string senderId, string receiverId, double amount);
    }
}