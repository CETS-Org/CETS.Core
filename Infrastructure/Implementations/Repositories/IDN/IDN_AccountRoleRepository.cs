using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.IDN
{
    public class IDN_AccountRoleRepository : BaseRepository<IDN_AccountRole>, IIDN_AccountRoleRepository
    {
        public IDN_AccountRoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<IDN_AccountRole>> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.IDN_AccountRoles
                .AsNoTracking()
                .Include(ar => ar.Role)
                .Where(ar => ar.AccountID == accountId)
                .ToListAsync();
        }

        public async Task<IDN_AccountRole?> GetByIdAsync(Guid accountId, Guid roleId)
        {
            return await _context.IDN_AccountRoles
                .AsNoTracking()
                .Include(ar => ar.Role)
                .FirstOrDefaultAsync(ar => ar.AccountID == accountId && ar.RoleID == roleId);
        }

        public async Task<IReadOnlyList<IDN_AccountRole>> GetByRoleIdAsync(Guid roleId)
        {
            return await _context.IDN_AccountRoles
                .AsNoTracking()
                .Include(ar => ar.Account)
                .Where(ar => ar.RoleID == roleId)
                .ToListAsync();
        }
    }
}


