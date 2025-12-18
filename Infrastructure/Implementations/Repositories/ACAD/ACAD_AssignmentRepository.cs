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

        public async Task<IEnumerable<UpcomingAssignmentResponse>> GetUpcomingAssignmentsForStudentAsync(Guid studentId, int limit = 5)
        {
            var now = DateTime.Now;
            
            var enrolledClassIds = await _context.ACAD_Enrollments
                .Where(e => e.StudentID == studentId && !e.IsDeleted && e.ClassID != null)
                .Select(e => e.ClassID!.Value)
                .ToListAsync();

            if (!enrolledClassIds.Any())
            {
                return Enumerable.Empty<UpcomingAssignmentResponse>();
            }

            
            var assignmentsData = await _context.ACAD_Assignments
                .Include(a => a.ClassMeeting)
                    .ThenInclude(cm => cm!.Class)
                .Include(a => a.ACAD_Submissions.Where(s => s.StudentID == studentId && !s.IsDeleted))
                .Where(a => 
                    !a.IsDeleted &&
                    a.ClassMeetingID != null &&
                    a.ClassMeeting != null &&
                    !a.ClassMeeting.IsDeleted &&
                    enrolledClassIds.Contains(a.ClassMeeting.ClassID) &&
                    a.DueAt != null &&
                    a.DueAt >= now)
                // Order by: pending first (no submissions), then by due date ascending
                .OrderBy(a => a.ACAD_Submissions.Any(s => s.StudentID == studentId && !s.IsDeleted) ? 1 : 0)
                .ThenBy(a => a.DueAt)
                .Take(limit)
                .ToListAsync();

            if (!assignmentsData.Any())
            {
                return Enumerable.Empty<UpcomingAssignmentResponse>();
            }

            // Get class IDs from the assignments to fetch session numbers
            var classIds = assignmentsData.Select(a => a.ClassMeeting!.ClassID).Distinct().ToList();
            
            // Get all meetings for these classes to calculate session numbers
            var classMeetings = await _context.ACAD_ClassMeetings
                .Where(cm => classIds.Contains(cm.ClassID) && !cm.IsDeleted)
                .OrderBy(cm => cm.Date)
                .ThenBy(cm => cm.SlotID)
                .Select(cm => new { cm.Id, cm.ClassID, cm.Date })
                .ToListAsync();

            // Build session number lookup: for each class, meetings ordered by date get session 1, 2, 3...
            var sessionNumberLookup = classMeetings
                .GroupBy(cm => cm.ClassID)
                .SelectMany(g => g.Select((cm, index) => new { cm.Id, SessionNumber = index + 1 }))
                .ToDictionary(x => x.Id, x => x.SessionNumber);

            // Map to response with session numbers
            var upcomingAssignments = assignmentsData.Select(a => new UpcomingAssignmentResponse
            {
                Id = a.Id,
                Title = a.Title ?? string.Empty,
                DueAt = a.DueAt,
                ClassName = a.ClassMeeting!.Class.ClassName ?? string.Empty,
                ClassId = a.ClassMeeting.ClassID,
                ClassMeetingId = a.ClassMeetingID!.Value,
                SessionNumber = sessionNumberLookup.GetValueOrDefault(a.ClassMeetingID!.Value, 0),
                HasSubmission = a.ACAD_Submissions.Any(s => s.StudentID == studentId && !s.IsDeleted),
                IsOverdue = a.DueAt < now
            }).ToList();

            return upcomingAssignments;
        }

    }
}


