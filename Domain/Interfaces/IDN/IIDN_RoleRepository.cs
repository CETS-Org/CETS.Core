using Domain.Entities;

namespace Domain.Interfaces.IDN
{
    public interface IIDN_RoleRepository : IBaseRepository<IDN_Role>
    {
        Task<Guid> GetRoleIdByNameAsync(string roleName);
        Task<IReadOnlyList<IDN_Role>> SearchRolesByKeywordAsync(string keyword);
    }
}


