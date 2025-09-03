using Domain.Entities;
using DTOs.COM_Feedback.Responses;
using DTOs.COM_FeedbackRecord.Requests;
using DTOs.COM_FeedbackRecord.Responses;

namespace Application.Interfaces.COM
{
	public interface ICOM_FeedbackRecordService : IBaseService<COM_FeedbackRecord, FeedbackRecordResponse, UpdateFeedbackRecordRequest, CreateFeedbackRecordRequest>
	{
        Task<FeedbackRecordResponse> SoftDeleteAsync(Guid id);

    }
}



