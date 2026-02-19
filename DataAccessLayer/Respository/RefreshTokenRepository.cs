using DataAccessLayer.Respository.IRepository;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DataAccessLayer.Respository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {

        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(DbContext.ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string tokenHash)
        {

            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash && !rt.IsRevoked);

        }


        public async Task RevokeAllForUserAsync(int userId)
        {

            var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            _context.RefreshTokens.UpdateRange(tokens);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(RefreshToken token)
        {

            _context.RefreshTokens.Update(token);

            await _context.SaveChangesAsync();
        }


        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);

            await _context.SaveChangesAsync();
        }


    }
}
