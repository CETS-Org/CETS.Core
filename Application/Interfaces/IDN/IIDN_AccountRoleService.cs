using DTOs.IDN_AccountRole.Requests;
using DTOs.IDN_AccountRole.Responses;

namespace Application.Interfaces.IDN
{
    public interface IIDN_AccountRoleService
    {
        Task<IReadOnlyList<AccountRoleResponse>> GetRolesByAccountIdAsync(Guid accountId);
        Task<AccountRoleResponse> AssignRoleAsync(AssignRoleRequest request);
        Task<bool> UnassignRoleAsync(UnassignRoleRequest request);
    }
}


