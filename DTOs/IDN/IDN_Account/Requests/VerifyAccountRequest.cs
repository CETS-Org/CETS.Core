using System.ComponentModel.DataAnnotations;

namespace DTOs.IDN.IDN_Account.Requests
{
    public class VerifyAccountRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Verification code is required")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Verification code must be exactly 6 characters")]
        public string VerificationCode { get; set; } = null!;
    }
}
