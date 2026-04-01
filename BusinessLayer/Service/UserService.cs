using BusinessLayer.DTOs.User;
using BusinessLayer.Security;
using BusinessLayer.Service.IService;
using DataAccessLayer.Entities;
using DataAccessLayer.Respository.IRepository;

namespace BusinessLayer.Service
{
    public class UserService : GenericService<TbUser>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReadOnlyRepostitory<TbUser> _readOnlyRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IReadOnlyRepostitory<TbUser> readOnlyRepository,
            IPasswordHasher passwordHasher) : base(userRepository)
        {
            _unitOfWork = unitOfWork;
            _readOnlyRepository = readOnlyRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task Add(CreateUserDto dto)
        {
            var newUser = new TbUser
            {
                UserName = dto.UserName,
                Password = _passwordHasher.HashPassword(dto.Password), // BCrypt hash only
                IsActive = dto.IsActive,
                PersonId = dto.ReterivePersonDto.Id,
            };

            await _unitOfWork.User.AddAsync(newUser);
        }

        public async Task Update(int userId, UpdateUserDto dto)
        {
            var userFromDb = await _unitOfWork.User.GetAsync(
                u => u.UserId == userId,
                includeProperties: null,
                tracked: true);

            if (userFromDb == null)
                throw new KeyNotFoundException($"User with ID {userId} was not found.");

            userFromDb.UserName = dto.UserName;
            userFromDb.Password = _passwordHasher.HashPassword(dto.Password); // BCrypt hash only
            userFromDb.IsActive = dto.IsActive;

            _unitOfWork.User.Update(userFromDb);
        }

        public bool IsUserExist(int userId)
        {
            return _unitOfWork.User.IsUserExist(userId);
        }

        public Task<IEnumerable<TbUser>> GetAllAsync()
        {
            return _readOnlyRepository.GetAllAsync();
        }
    }
}