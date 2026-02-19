using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository.IRepository
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {

        Task AddAsync(RefreshToken token);

        Task UpdateAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenAsync(string tokenHash);
        Task RevokeAllForUserAsync(int userId);

    }
}
