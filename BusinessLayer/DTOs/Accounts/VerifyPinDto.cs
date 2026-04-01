using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.Accounts
{

    public class VerifyPinDto
    {
        [Required(ErrorMessage = "Account number is required.")]
        public string AccountNumber { get; set; } = null!;

        [Required(ErrorMessage = "PIN is required.")]
        public string Pin { get; set; } = null!;
    }
}