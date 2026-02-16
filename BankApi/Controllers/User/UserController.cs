using BusinessLayer.DTOs.People;
using BusinessLayer.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBank.Controllers.User
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var lstUser = await _userService.GetAllAsync();

                return Ok(new { success = true, data = lstUser });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving Person." });
            }
        }


        [HttpGet("GetByID{userId}", Name = "GetUserByID")]
        public async Task<IActionResult> GetUserByID(int userId)
        {

            try
            {

                if (userId < 0)
                {
                    return BadRequest($"Not Accepted ID: {userId}");
                }

                var user = await _userService.GetAsync(u => u.UserId == userId, "Person,Person.NationalityCountry");

                if (user != null)
                {
                    ReteriveUserDto reteriveUserdto = new ReteriveUserDto()
                    {
                        userId = userId,
                        UserName = user.UserName,
                       FullName= user.Person.FirstName+' ' + user.Person.LastName,
                        IsActive = user.IsActive,
                        DateOfBirth = user.Person.DateOfBirth,
                        CountryName = user.Person.NationalityCountry.CountryName
                    };

                    return Ok(new { success = true, data = reteriveUserdto });
                }

                else
                {
                    return NotFound($"User with ID {userId} Not Found.");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An Error Occurred while retriving User." });
            }
        }


        [HttpGet("IsExist{userId:int}", Name = "IsUserExist")]
        public async Task<IActionResult> IsExist(int userId)
        {

            if (userId < 0)
            {
                return BadRequest($"Not Accepted ID: {userId}");
            }

            if (_userService.IsUserExist(userId))
            {
                return Ok(new { success = true, data = "User is exist" });
            }

            return NotFound($"User with ID :{userId} is not found.");

        }


        [HttpPost(Name = "AddUser")]
        public async Task<IActionResult> AddPerson(CreateUserDto dto)
        {

            if (dto == null)
            {
                return BadRequest("Invalid Person data.");
            }

            await _userService.Add(dto);
            await _userService.SaveChanges();
            return Ok(dto);
        }


        [HttpPut(Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto dto)
        {
            
            if (userId < 0)
                return BadRequest($"Not Accepted ID: {userId}");

            if (!_userService.IsUserExist(userId))
                NotFound("Person is not Found exist..");

            await _userService.Update(userId, dto);
            await _userService.SaveChanges();

            return Ok(dto);
        }


        [HttpDelete("Delete{userId}", Name = "DeleteUser")]
        public async Task<IActionResult> DeletePerson(int userId)
        {

            if (userId < 0)
                return BadRequest($"Not Accepted ID {userId}");

            var user = await _userService.GetAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound($"User with ID {user} not found..");

            
            _userService.Remove(user);
            await _userService.SaveChanges();

            return Ok($"User with ID {userId} has been deleted.");
        }
    }
}
