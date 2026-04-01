using BusinessLayer.DTOs.User;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;

namespace BusinessLayer.Service.IService
{
    public interface IUserService : IGenericeService<TbUser>, IReadOnlyRepostitory<TbUser>
    {
        Task Add(CreateUserDto dto);

        Task Update(int userId, UpdateUserDto dto);

        bool IsUserExist(int userId);
    }
}