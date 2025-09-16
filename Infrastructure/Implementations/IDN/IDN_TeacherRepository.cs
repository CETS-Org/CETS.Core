using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.IDN
{
    public class IDN_TeacherRepository : BaseRepository<IDN_Teacher>, IIDN_TeacherRepository
    {
        public IDN_TeacherRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IDN_Teacher?> GetTeacherDetailsByIdAsync(Guid id)
        {
            return await _context.IDN_Teachers
                .Include(t => t.Account)
                    .ThenInclude(a => a.IDN_AccountRoles)
                        .ThenInclude(ar => ar.Role)
                .Include(t => t.IDN_TeacherCredentials)
                    .ThenInclude(tc => tc.CredentialType)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

    }
}


