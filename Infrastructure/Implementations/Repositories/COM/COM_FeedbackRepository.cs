using Domain.Data;
using Domain.Entities;
using Domain.Interfaces.COM;
using DTOs.COM.COM_Feedback.Responses;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations.Repositories.COM
{
    public class COM_FeedbackRepository : BaseRepository<COM_Feedback>, ICOM_FeedbackRepository
    {
        public COM_FeedbackRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<CourseFeedbackListResponse>> GetFeedbacksByCourseIdAsync(Guid courseId)
        {
            var feedbacks = await _context.COM_Feedbacks
                .Where(f => f.CourseID == courseId && !f.IsDeleted)
                .Include(f => f.Submitter)
                    .ThenInclude(s => s.Account)
                .Include(f => f.FeedbackType)
                .Include(f => f.Teacher)
                    .ThenInclude(t => t.Account)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            var result = feedbacks.Select(f => new CourseFeedbackListResponse
            {
                FeedbackId = f.Id,
                SubmitterId = f.SubmitterID,
                SubmitterName = f.Submitter?.Account?.FullName ?? "Unknown",
                FeedbackTypeId = f.FeedbackTypeID.ToString(),
                FeedbackTypeName = f.FeedbackType?.Name ?? "Unknown",
                Rating = f.Rating,
                Comment = f.Comment,
                ContentClarity = f.ContentClarity,
                CourseRelevance = f.CourseRelevance,
                MaterialsQuality = f.MaterialsQuality,
                TeacherId = f.TeacherID,
                TeacherName = f.Teacher?.Account?.FullName,
                TeachingEffectiveness = f.TeachingEffectiveness,
                CommunicationSkills = f.CommunicationSkills,
                TeacherSupportiveness = f.TeacherSupportiveness,
                CreatedAt = f.CreatedAt
            }).ToList();

            return result;
        }
    }
}


