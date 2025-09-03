using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.COM;
using DTOs.COM.COM_Feedback.Requests;
using DTOs.COM.COM_Feedback.Responses;

namespace Application.Implementations.COM
{
	public class COM_FeedbackService : BaseService<COM_Feedback, FeedbackResponse, UpdateFeedbackRequest, CreateFeedbackRequest>, ICOM_FeedbackService
	{
		public COM_FeedbackService(ICOM_FeedbackRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}

		public async Task<FeedbackResponse> SoftDeleteAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null)
			{
				throw new KeyNotFoundException($"COM_Feedback with id {id} not found.");
			}
			entity.IsDeleted = true;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<FeedbackResponse>(entity);
        }
    }
}



