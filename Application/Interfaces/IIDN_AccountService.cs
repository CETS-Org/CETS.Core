using DTOs.IDN_Account.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIDN_AccountService
    {
        Task<IReadOnlyList<AccountStatusDto>> GetAccountStatusesAsync();
    }
}
