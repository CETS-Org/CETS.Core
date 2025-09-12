using System.ComponentModel.DataAnnotations;

namespace DTOs.IDN.IDN_Account.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
        public string Password { get; set; } = null!;
    }
}
