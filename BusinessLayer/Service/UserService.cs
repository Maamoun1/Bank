using BusinessLayer.DTOs.User;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserService : GenericService<TbUser>, IUserService, IReadOnlyRepostitory<TbUser>
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IReadOnlyRepostitory<TbUser> _ReadOnlyRepository;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork,IReadOnlyRepostitory<TbUser> ReadOnlyRepository) : base(userRepository)
        {
            _unitOfWork = unitOfWork;
            _ReadOnlyRepository = ReadOnlyRepository;
        }

        public async Task Add(CreateUserDto dto)
        {
            var newUser = new TbUser
            {
                UserName = dto.UserName,
                Password = dto.Password,
                IsActive = dto.IsActive,
                PersonId = dto.reterivePersondto.Id
            };

            await _unitOfWork.User.AddAsync(newUser);
        }

        public bool IsUserExist(int UserId)
        {
            return (_unitOfWork.User.IsUserExist(UserId));

        }

        public async Task Update(int userId, UpdateUserDto dto)
        {
            var userFromDb = await _unitOfWork.User.GetAsync(u => u.UserId == userId);

            if (userFromDb != null)
            {

                userFromDb.UserName = dto.UserName;
                userFromDb.Password = dto.Password;
                userFromDb.IsActive = dto.IsActive;

                _unitOfWork.User.Update(userFromDb);
            }
        }

        public Task<IEnumerable<TbUser>> GetAllAsync()
        {
            return _ReadOnlyRepository.GetAllAsync();
        }


    }
}
