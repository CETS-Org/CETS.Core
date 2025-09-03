using Domain.Entities;

namespace Domain.Interfaces.IDN
{
    public interface IIDN_AccountRoleRepository : IBaseRepository<IDN_AccountRole>
    {
        Task<IReadOnlyList<IDN_AccountRole>> GetByAccountIdAsync(Guid accountId);
        Task<IDN_AccountRole?> GetByIdAsync(Guid accountId, Guid roleId);
        Task<IReadOnlyList<IDN_AccountRole>> GetByRoleIdAsync(Guid roleId);
    }
}


