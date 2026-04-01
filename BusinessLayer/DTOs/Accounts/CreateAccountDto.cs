using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.Accounts
{
    public class CreateAccountDto
    {
        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Issue reason is required.")]
        public string IssueReason { get; set; } = null!;

        public int? CreatedByUserId { get; set; }

 
        [MinLength(4, ErrorMessage = "PIN must be at least 4 characters.")]
        public string? InitialPin { get; set; }
    }
}