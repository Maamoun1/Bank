using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository.IRepository
{
    public interface IUserRepository : IGenericRepository<TbUser>
    {
        void Update(TbUser entity);

        bool IsUserExist(int UserId);

        Task<bool> UsernameExistsAsync(string username);
        Task<TbUser?> GetByUsernameAsync(string username);
    }
}
