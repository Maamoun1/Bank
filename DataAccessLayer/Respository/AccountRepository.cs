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

        public bool IsAccountExist(string pinCode)
        {
            return _context.Accounts.Where(a => a.PinCode == pinCode).Any();
        }

        public void Update(TbAccount account)
        {
            _context.Update(account);
        }
    }
}
