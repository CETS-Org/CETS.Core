using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.COM;
using DTOs.COM_FeedbackRecord.Requests;
using DTOs.COM_FeedbackRecord.Responses;

namespace Application.Implementations.COM
{
	public class COM_FeedbackRecordService : BaseService<COM_FeedbackRecord, FeedbackRecordResponse, UpdateFeedbackRecordRequest, CreateFeedbackRecordRequest>, ICOM_FeedbackRecordService
	{
		public COM_FeedbackRecordService(ICOM_FeedbackRecordRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}

		public async Task<FeedbackRecordResponse> SoftDeleteAsync(Guid id)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null)
			{
				throw new KeyNotFoundException($"COM_FeedbackRecord with id {id} not found.");
			}
			entity.IsDeleted = true;
			_repository.Update(entity);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<FeedbackRecordResponse>(entity);
        }
    }
}



