using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.Accounts
{

    public class UpdatePinDto
    {
        [Required(ErrorMessage = "Current PIN is required.")]
        public string CurrentPin { get; set; } = null!;

        [Required(ErrorMessage = "New PIN is required.")]
        [MinLength(4, ErrorMessage = "PIN must be at least 4 characters.")]
        public string NewPin { get; set; } = null!;
    }
}