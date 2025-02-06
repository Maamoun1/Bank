using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository.IRepository
{
    public interface IApplicationsRepository : IGenericRepository<TbApplication>
    {

        void UpdateStatus(TbApplication application,short status);

        Task SaveChanges();



    }
}
