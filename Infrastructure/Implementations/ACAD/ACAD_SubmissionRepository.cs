using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.ACAD
{
    public class ACAD_SubmissionRepository : BaseRepository<ACAD_Submission>, IACAD_SubmissionRepository
    {
        public ACAD_SubmissionRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ACAD_Submission>> GetByAssignmentAsync(Guid assignmentId)
        {
            return await _context.ACAD_Submissions
                .Where(s => s.AssignmentID == assignmentId && !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<ACAD_Submission>> GetByStudentAsync(Guid studentId)
        {
            return await _context.ACAD_Submissions
                .Where(s => s.StudentID == studentId && !s.IsDeleted)
                .ToListAsync();
        }
        public async Task<int> CountByStudentAsync(Guid studentId)
        {
            return await _context.ACAD_Submissions
                .CountAsync(s => s.StudentID == studentId && !s.IsDeleted);
        }
    }
}


