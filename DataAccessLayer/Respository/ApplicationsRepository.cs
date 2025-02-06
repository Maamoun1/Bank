using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository
{
    public class ApplicationsRepository : GenericRepository<TbApplication>, IApplicationsRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateStatus(TbApplication application,short status)
        {
            _context.Applications.Update(application);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

    }
}
