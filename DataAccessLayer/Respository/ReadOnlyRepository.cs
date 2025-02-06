using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository
{
    public class ReadOnlyRepository<T> : IReadOnlyRepostitory<T> where T : class
    {

        private readonly ApplicationDbContext _context;

        private readonly DbSet<T> _dbSet;
        public ReadOnlyRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }


    }
}
