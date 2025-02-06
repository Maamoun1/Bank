using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository.IRepository
{
    public interface IAccountsTypesRepository
    {
        Task<IEnumerable> GetAllAsync();

    }
}
