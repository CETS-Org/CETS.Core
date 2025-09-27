using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.IDN;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.IDN
{
    public class IDN_StudentRepository : BaseRepository<IDN_Student>, IIDN_StudentRepository
    {
        public IDN_StudentRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IDN_Student?> GetStudentWithAccountAsync(Guid accountId)
        {
            return await _context.IDN_Students
                .Include(s => s.Account)
                .FirstOrDefaultAsync(s => s.Account.Id == accountId);
        }
        public async Task<IDN_Student?> GetStudentWithStudentCode(string studentCode)
        {
            return await _context.IDN_Students
                .Include(s => s.Account)
                .FirstOrDefaultAsync(s => s.StudentCode == studentCode);
        }
    }
}


