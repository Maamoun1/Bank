using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository
{
    public class AccountRepository : GenericRepository<TbAccount>, IAccountRepository
    {

        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task DecativeAccount(int accountId)
        {
            throw new NotImplementedException();
        }

        public  async Task DepositeAsync(string accountNumber, decimal balance)
        {

            var account =  await _context.Accounts.SingleOrDefaultAsync(p => p.AccountNumber == accountNumber);

            if (account != null)
            {
                account.Balance += balance;
            }
            else
               throw new Exception($"not found accountNumber with {accountNumber}");
        }

        public async Task<double?> GetBalanceAsync(string accountNumber)
        {

            return await _context.Accounts
                .AsNoTracking()
                .Where(a => a.AccountNumber == accountNumber)
                .Select(a => (double?)a.Balance)
                .FirstOrDefaultAsync();

        }

        public async Task <bool> IsAccountExist(string pinCode)
        {
            return await _context.Accounts.AnyAsync(a => a.AccountNumber == pinCode);
        }

        public async Task<bool> TransferAmountAsync(string senderId, string receviedId, decimal amount)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sender = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == senderId);
                var recevier = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == receviedId);

                if (sender == null || recevier == null)
                    throw new Exception("one or both account not found.");

                if (sender.Balance < amount)
                    throw new Exception("Insufficient funds.");

                //Deduct from sender
                sender.Balance -= amount;
                _context.Accounts.Update(sender);

                //Add to receiver
                recevier.Balance += amount;
                _context.Accounts.Update(recevier);

                //save changed
                await _context.SaveChangesAsync();

                //commit transaction
                await transaction.CommitAsync();
                return true;
            }

            catch (Exception ex)
            {
                // Rollback if anything goes wrong
                await transaction.RollbackAsync();
                Console.WriteLine($"Transaction failed: {ex.Message}");
                return false;
            }

        }

        public void Update(TbAccount account)
        {
            _context.Update(account);
        }

        public async Task WithdrawAsync(string accountNumber, decimal balance)
        {

            var account = await _context.Accounts
        .SingleOrDefaultAsync(p => p.AccountNumber == accountNumber);

            if (account == null)
                throw new Exception($"Account not found: {accountNumber}");

            if (account.Balance < balance)
                throw new InvalidOperationException("Insufficient funds.");

            account.Balance -= balance;

        }
    }
}
