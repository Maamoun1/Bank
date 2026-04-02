using BusinessLayer.DTOs.Accounts;
using BusinessLayer.Global_Class;
using BusinessLayer.Security;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;

namespace BusinessLayer.Service
{
    public class AccountService : GenericService<TbAccount>, IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cache;
        private readonly IPasswordHasher _passwordHasher;

        public AccountService(
            IAccountRepository accountRepository,
            IUnitOfWork unitOfWork,
            ICacheService cache,
            IPasswordHasher passwordHasher) : base(accountRepository)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _passwordHasher = passwordHasher;
        }

        public async Task AddAccount(CreateAccountDto dto)
        {
            // Caller provides a PIN or the service generates one.
            // Either way only the BCrypt hash is persisted — plaintext is
            // discarded after hashing and must be given to the client via
            // a secure out-of-band channel by the calling layer.
            var plainPin = string.IsNullOrWhiteSpace(dto.InitialPin)
                ? GenerateRandomPin()
                : dto.InitialPin;

            var newAccount = new TbAccount
            {
                ApplicationId = dto.ApplicationId,
                ClientId = dto.ClientId,
                AccountNumber = Util.GenerateAccountNumber(5),
                PinHash = _passwordHasher.HashPassword(plainPin),
                IssueDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddYears(3),
                IssueReason = dto.IssueReason,
                Balance = 0,
                IsActive = true,
                CreatedByUserId = dto.CreatedByUserId,
            };

            await _unitOfWork.Account.AddAsync(newAccount);
        }

        public async Task<bool> VerifyPinAsync(string accountNumber, string submittedPin)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(submittedPin))
                return false;

            // tracked: true so EF returns the live entity with PinHash populated.
            var account = await _unitOfWork.Account.GetAsync(
                a => a.AccountNumber == accountNumber,
                includeProperties: null,
                tracked: true);

            if (account == null || !account.IsActive)
                return false;

            return _passwordHasher.VerifyPassword(submittedPin, account.PinHash);
        }

        public async Task UpdatePinAsync(string accountNumber, UpdatePinDto dto)
        {
            var account = await _unitOfWork.Account.GetAsync(
                a => a.AccountNumber == accountNumber,
                includeProperties: null,
                tracked: true);

            if (account == null)
                throw new KeyNotFoundException($"Account '{accountNumber}' was not found.");

            if (!_passwordHasher.VerifyPassword(dto.CurrentPin, account.PinHash))
                throw new InvalidOperationException("Current PIN is incorrect.");

            account.PinHash = _passwordHasher.HashPassword(dto.NewPin);
            _unitOfWork.Account.Update(account);
        }

 
        public async Task<double> GetBalanceAsync(string accountNumber)
        {
            var cacheKey = CacheKeys.Balance(accountNumber);

            var cachedBalance = await _cache.GetAsync<double?>(cacheKey);
            if (cachedBalance != null)
                return cachedBalance.Value;

            var balance = await _unitOfWork.Account.GetBalanceAsync(accountNumber);

            if (balance is null)
                throw new KeyNotFoundException($"Account '{accountNumber}' was not found.");

            await _cache.SetAsync(cacheKey, balance.Value, TimeSpan.FromMinutes(5));

            return balance.Value;
        }

        public async Task DepositeAsync(string accountNumber, decimal balance)
        {
            if (balance < 100)
                throw new ArgumentException("Deposit amount must be at least 100.");

            if (!await IsAccountExistAsync(accountNumber))
                throw new KeyNotFoundException($"Account '{accountNumber}' was not found.");

            await _unitOfWork.Account.DepositeAsync(accountNumber, balance);
            await _unitOfWork.SaveAsync();

            await _cache.RemoveAsync(CacheKeys.Balance(accountNumber));
        }

        public async Task WithdrawAsync(string accountNumber, decimal balance)
        {
            if (balance < 100)
                throw new ArgumentException("Withdrawal amount must be at least 100.");

            if (!await IsAccountExistAsync(accountNumber))
                throw new KeyNotFoundException($"Account '{accountNumber}' was not found.");

            await _unitOfWork.Account.WithdrawAsync(accountNumber, balance);
            await _unitOfWork.SaveAsync();

            await _cache.RemoveAsync(CacheKeys.Balance(accountNumber));
        }


        public async Task<bool> TransferAmountAsync(string senderId, string receiverId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be greater than zero.");

            return await _unitOfWork.Account.TransferAmountAsync(senderId, receiverId, amount);
        }

        public async Task<bool> IsAccountExistAsync(string accountNumber)
        {
            return await _unitOfWork.Account.IsAccountExist(accountNumber);
        }

        public Task DecativeAccount(int accountId)
        {
            throw new NotImplementedException();
        }


        private static string GenerateRandomPin(int length = 6)
        {
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            // Map each byte to a digit 0-9
            return string.Concat(bytes.Select(b => (b % 10).ToString()));
        }
    }
}