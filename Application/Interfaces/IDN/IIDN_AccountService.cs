using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Account.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDN
{
    public interface IIDN_AccountService
    {
        Task<IReadOnlyList<AccountStatusResponse>> GetAccountStatusesAsync();
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync(AccountFilterRequest filter);
        Task<AccountResponse?> GetAccountByIdAsync(Guid id);
        Task<AccountResponse> GetAccountByEmailAsync(string email);
        Task<AccountResponse> UpdateAccountAsync(Guid id, UpdateAccountRequest dto);
        Task<AccountResponse?> DeactivateAccountAsync(Guid id);
        Task<AccountResponse?> ActivateAccountAsync(Guid id);
        Task<AccountResponse> SoftDeleteAccountAsync(Guid id);
        Task<AccountResponse> RestoreAccountAsync(Guid id);
    }
}
