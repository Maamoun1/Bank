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

        Task<TbUser?> GetByIdAsync(int id);


        bool IsUserExist(int UserId);

        Task<bool> UsernameExistsAsync(string username);
        Task<TbUser?> GetByUsernameAsync(string username);
    }
}
