using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.IDN
{
    public class IDN_RoleRepository : BaseRepository<IDN_Role>, IIDN_RoleRepository
    {
        public IDN_RoleRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Guid> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _context.IDN_Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleName == roleName);

            if (role == null)
                throw new InvalidOperationException($"Role '{roleName}' not found.");

            return role.Id;
        }

        public async Task<IReadOnlyList<IDN_Role>> SearchRolesByKeywordAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<IDN_Role>();

            keyword = keyword.ToLower();

            return await _context.IDN_Roles
                .Where(r => r.RoleName.ToLower().Contains(keyword))
                .ToListAsync();
        }

    }
}


