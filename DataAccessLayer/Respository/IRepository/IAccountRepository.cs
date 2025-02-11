using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository.IRepository
{
    public interface IAccountRepository : IGenericRepository<TbAccount>
    {
        Task DecativeAccount(int  accountId);

        bool IsAccountExist(string pinCode);
        void Update(TbAccount account); 

    }
}
