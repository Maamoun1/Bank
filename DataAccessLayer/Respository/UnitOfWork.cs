using DataAccessLayer.DbContext;
using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DbContext.ApplicationDbContext _context;

        public UnitOfWork(DbContext.ApplicationDbContext context)
        {
            _context = context;

            Person = new PersonRepository(_context);

            User = new UserRepository(_context);

            Applications = new ApplicationsRepository(_context);
        }

        public IPersonRepostitory Person { get; private set; }

        public IUserRepository User {  get; private set; }

        public IAccountsTypesRepository AccountType { get; private set; }

        public IApplicationsRepository Applications { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
