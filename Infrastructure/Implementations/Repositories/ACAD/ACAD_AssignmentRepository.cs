using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
{
    public class ACAD_AssignmentRepository : BaseRepository<ACAD_Assignment>, IACAD_AssignmentRepository
    {
        public ACAD_AssignmentRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_Assignment>> GetByClassMeetingAsync(Guid classMeetingId)
        {
            return await _context.ACAD_Assignments
                .Where(a => a.ClassMeetingID == classMeetingId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_Assignment>> GetByTeacherAsync(Guid teacherId)
        {
            return await _context.ACAD_Assignments
                .Where(a => a.CreatedBy == teacherId && !a.IsDeleted)
                .ToListAsync();
        }
    }
}


