using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.IDN.IDN_Authentication;

namespace Application.Interfaces.IDN
{
    public interface IIDN_JwtService
    {
        string GenerateJwtToken<T>(T user) where T : IAppUser;
        bool ValidateJwtToken(string token);
        string GenerateOtpJwt(string email, string otp);
        bool ValidateOtpJwt(string token, string emailInput, string otpInput);
        bool ValidatePasswordResetToken(string token, string emailInput);
    }
}
