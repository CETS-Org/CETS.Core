using Domain.Entities;
using DTOs.COM.COM_Feedback.Responses;

namespace Domain.Interfaces.COM
{
    public interface ICOM_FeedbackRepository : IBaseRepository<COM_Feedback>
    {
        Task<List<CourseFeedbackListResponse>> GetFeedbacksByCourseIdAsync(Guid courseId);
    }
}


