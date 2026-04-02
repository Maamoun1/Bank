using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs.Accounts
{

    public class TransferRequestDto
    {
        [Required(ErrorMessage = "Sender account number is required.")]
        public string SenderId { get; set; } = null!;

        [Required(ErrorMessage = "Receiver account number is required.")]
        public string ReceiverId { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Transfer amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}