using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.ACAD;
using DTOs.ACAD.ACAD_Assignment.Responses;
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

        public async Task<IEnumerable<ACAD_Assignment>> GetAssignmentsWithSubmissions(Guid classMeetingId, Guid studentId)
        {
            return await _context.ACAD_Assignments
                .Include(a => a.ACAD_Submissions.Where(s => s.StudentID == studentId && !s.IsDeleted))
                .Include(a => a.Skill)
                .Where(a => a.ClassMeetingID == classMeetingId && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssignmentWithSubmissionCountResponse>> GetAssignmentsWithSubmissionCountAsync(Guid classMeetingId)
        {
            var assignments = await _context.ACAD_Assignments
                .Include(a => a.Skill)
                .Where(a => a.ClassMeetingID == classMeetingId && !a.IsDeleted)
                .Select(a => new AssignmentWithSubmissionCountResponse
                {
                    Id = a.Id,
                    ClassMeetingId = a.ClassMeetingID.HasValue ? a.ClassMeetingID.Value : Guid.Empty,
                    Title = a.Title ?? string.Empty,
                    Description = a.Description,
                    StoreUrl = a.StoreUrl,
                    DueAt = a.DueAt,
                    CreatedAt = a.CreatedAt,
                    SubmissionCount = a.ACAD_Submissions.Count(s => !s.IsDeleted),
                    SkillID = a.SkillID,
                    SkillName = a.Skill != null ? a.Skill.Name : null
                })
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return assignments;
        }

    }
}


