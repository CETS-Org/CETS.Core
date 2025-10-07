using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.ACAD
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
                .Include(x => x.Student).ThenInclude(s => s.Account)
                .Include(x => x.Assignment)
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

        public async Task<(int submitted, int total)> GetSubmissionSummaryAsync(Guid studentId, Guid courseId)
        {
            var totalAssignments = await _context.ACAD_Assignments
                .Where(a => a.ClassMeeting.Class.Id ==
                    _context.ACAD_Enrollments
                        .Where(e => e.StudentID == studentId && e.CourseID == courseId)
                        .Select(e => e.ClassID)
                        .FirstOrDefault())
                .CountAsync();

            var submitted = await _context.ACAD_Submissions
                .Where(s => s.StudentID == studentId && !s.IsDeleted)
                .CountAsync();

            return (submitted, totalAssignments);
        }

        public async Task<ACAD_Submission?> GetStudentSubmissionByAssignmentID(Guid studentId, Guid assignmentId)
        {
            return await _context.ACAD_Submissions.Include(s => s.Assignment).Where(s=> s.Id == studentId && s.AssignmentID == assignmentId).FirstOrDefaultAsync();
        }
    }
}


