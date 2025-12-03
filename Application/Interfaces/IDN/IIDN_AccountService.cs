using DTOs.COM.COM_Chat.Responses;
using DTOs.IDN.IDN_Account.Requests;
using DTOs.IDN.IDN_Account.Responses;
using DTOs.IDN.IDN_Teacher.Requests;
using DTOs.IDN.IDN_Teacher.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IDN
{
    public interface IIDN_AccountService
    {
        Task<IReadOnlyList<AccountStatusResponse>> GetAccountStatusesAsync();
        Task<IEnumerable<AccountResponse>> GetAllAccountsAsync(AccountFilterRequest filter);
        Task<AccountResponse> CreateAccountAsync(CreateAccountRequest dto);
        Task<AccountResponse?> GetAccountByIdAsync(Guid id);
        Task<AccountResponse> GetAccountByEmailAsync(string email);
        Task<AccountResponse> UpdateAccountAsync(Guid id, UpdateAccountRequest dto);
        Task<AccountResponse?> UpdateAccountProfileAsync(Guid accountId, UpdateAccountProfileRequest dto, ClaimsPrincipal user);
        Task<AccountResponse?> DeactivateAccountAsync(Guid id);
        Task<AccountResponse?> ActivateAccountAsync(Guid id);
        Task<AccountResponse> SoftDeleteAccountAsync(Guid id);
        Task<AccountResponse> RestoreAccountAsync(Guid id);
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsPhoneNumberExistsAsync(string phoneNumber);
        Task<bool> IsPhoneUniqueAsync(string phoneNumber);
        Task<LoginAccountResponse?> ValidateUserCredentialsAsync(string email, string password);
        Task<LoginAccountResponse> ValidateGoogleAccountAsync(GoogleLoginRequest googleLoginRequest);
        Task<bool> VerifyAccountAsync(VerifyAccountRequest dto);
        Task<bool> ResendVerificationCodeAsync(string email);
        Task<AccountResponse> RegisterAsync(RegisterRequest dto);
        Task<string?> GetOTP(string email);
        bool VerifyOTP(VerifyOtpRequest dto);
        Task<bool> ChangePassword(string password, string email);

        Task<bool> CheckEmailExist(string email);
        Task<bool> CheckPhoneExist(string phoneNumber);
        Task<bool> CheckCIDExist(string cid);
        Task<List<UserBasicInfo>> GetUsersByIdsAsync(List<Guid> userIds);
    }
}
