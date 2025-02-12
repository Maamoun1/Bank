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

        public   void Deposite(string pinCode, double balance)
        {

            var account = _context.Accounts.FirstOrDefault(p => p.AccountNumber == pinCode);

            if (account != null)
            {
                account.Balance += balance;
            }
            else
                Console.WriteLine($"not found pincode with {pinCode}");
        }

        public double GetBalance(string accountNumber)
        {
            return _context.Accounts.Where(p => p.AccountNumber == accountNumber).Select(p => p.Balance).FirstOrDefault();
        }
        public bool IsAccountExist(string pinCode)
        {
            return _context.Accounts.Where(a => a.AccountNumber == pinCode).Any();
        }

        public async Task<bool> TransferAmountAsync(string senderId, string receviedId, double amount)
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

        public void Withdraw(string pinCode, double balance)
        {

            Deposite(pinCode, balance * -1);
        }
    }
}
