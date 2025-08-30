using DTOs.IDN_Account.Requests;
using DTOs.IDN_Account.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDN
{
    public interface IIDN_AccountService
    {
        Task<IReadOnlyList<AccountStatusResponse>> GetStatusesAsync();
        Task<IReadOnlyList<AccountResponse>> GetAllAsync();
        Task<AccountResponse?> GetByIdAsync(Guid id);
        Task<AccountResponse> GetByEmailAsync(string email);
        Task<AccountResponse?> UpdateAsync(Guid id, UpdateAccountRequest dto);
        Task<AccountResponse?> DeactivateAccountAsync(Guid id);
    }
}
