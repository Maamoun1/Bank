using BusinessLayer.DTOs.People;
using BusinessLayer.DTOs.User;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service.IService
{
    public interface IUserService : IGenericeService<TbUser>, IReadOnlyRepostitory<TbUser>
    {
        Task Update(int userId, UpdateUserDto dto);
        Task Add(CreateUserDto dto);

        bool IsUserExist(int UserId);

    }
}
