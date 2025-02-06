using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository
{
    public class AccountTypeRepository : IAccountsTypesRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<TbAccountsType> _dbSet;
        public AccountTypeRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TbAccountsType>();
        }

        public async Task<IEnumerable> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
    }
}
