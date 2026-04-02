using BusinessLayer.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiBank.Controllers.User
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(new { success = true, data = users });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving users." });
            }
        }

        [HttpGet("GetByID/{userId}", Name = "GetUserByID")]
        public async Task<IActionResult> GetUserByID(int userId)
        {
            var authResult = await _authorizationService.AuthorizeAsync(User, userId, "SameUserPolicy");
            if (!authResult.Succeeded)
                return Unauthorized();

            try
            {
                if (userId < 0)
                    return BadRequest(new { success = false, message = $"Invalid ID: {userId}" });

                var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out int currentUserId))
                    return Unauthorized(new { success = false, message = "Invalid user identity." });

                if (userId != currentUserId && !User.IsInRole("Admin"))
                    return StatusCode(403, new { success = false, message = "Forbidden: You cannot access another user's information." });

                var user = await _userService.GetAsync(u => u.UserId == userId, "Person,Person.NationalityCountry");

                if (user == null)
                    return NotFound(new { success = false, message = $"User with ID {userId} not found." });

                var dto = new ReteriveUserDto
                {
                    userId = userId,
                    UserName = user.UserName,
                    FullName = user.Person.FirstName + " " + user.Person.LastName,
                    IsActive = user.IsActive,
                    DateOfBirth = user.Person.DateOfBirth,
                    CountryName = user.Person.NationalityCountry.CountryName,
                };

                return Ok(new { success = true, data = dto });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving the user." });
            }
        }

        [HttpGet("IsExist/{userId:int}", Name = "IsUserExist")]
        public IActionResult IsExist(int userId)
        {
            if (userId < 0)
                return BadRequest(new { success = false, message = $"Invalid ID: {userId}" });

            var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out int currentUserId))
                return Unauthorized(new { success = false, message = "Invalid user identity." });

            if (userId != currentUserId && !User.IsInRole("Admin"))
                return StatusCode(403, new { success = false, message = "Forbidden: You cannot access another user's information." });

            if (_userService.IsUserExist(userId))
                return Ok(new { success = true, message = "User exists." });

            return NotFound(new { success = false, message = $"User with ID {userId} not found." });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddUser")]
        public async Task<IActionResult> AddUser([FromBody] CreateUserDto dto)
        {
            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid user data." });

            await _userService.Add(dto);
            await _userService.SaveChanges();

            // Never return the DTO — it contains the plain-text password supplied by the caller.
            return Ok(new { success = true, message = "User created successfully." });
        }


        [HttpPut("{userId}", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto dto)
        {
            if (userId < 0)
                return BadRequest(new { success = false, message = $"Invalid ID: {userId}" });

            if (dto == null)
                return BadRequest(new { success = false, message = "Invalid user data." });

            try
            {
                await _userService.Update(userId, dto);
                await _userService.SaveChanges();

                return Ok(new { success = true, message = "User updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating the user." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{userId}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if (userId < 0)
                return BadRequest(new { success = false, message = $"Invalid ID: {userId}" });

            var user = await _userService.GetAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound(new { success = false, message = $"User with ID {userId} not found." });

            _userService.Remove(user);
            await _userService.SaveChanges();

            return Ok(new { success = true, message = $"User with ID {userId} has been deleted." });
        }
    }
}