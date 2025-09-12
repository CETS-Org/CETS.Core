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
    }
}
