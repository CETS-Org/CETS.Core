using System.ComponentModel.DataAnnotations;

namespace DTOs.IDN.IDN_Account.Requests
{
    public class ResendVerificationRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;
    }
}
