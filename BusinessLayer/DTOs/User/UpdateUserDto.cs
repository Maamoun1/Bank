using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.User
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(30, ErrorMessage = "Username cannot exceed 30 characters.")]
        public string UserName { get; set; } = null!;


        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}