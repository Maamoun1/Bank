using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Respository
{
    public class UserRepository : GenericRepository<TbUser> , IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(DbContext.ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public bool IsUserExist(int UserId)
        {
            //  return _context.People.Find(PersonID) != null;
            return _context.Users.Where(u => u.UserId == UserId).Any();
        }
        public  void Update(TbUser entity)
        {
            _context.Users.Update(entity);
        }



    }
}
