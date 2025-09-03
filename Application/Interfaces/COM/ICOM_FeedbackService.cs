using Domain.Entities;
using DTOs.COM_Feedback.Requests;
using DTOs.COM_Feedback.Responses;

namespace Application.Interfaces.COM
{
	public interface ICOM_FeedbackService : IBaseService<COM_Feedback, FeedbackResponse, UpdateFeedbackRequest, CreateFeedbackRequest>
	{
		Task<FeedbackResponse> SoftDeleteAsync(Guid id);
    }
}



