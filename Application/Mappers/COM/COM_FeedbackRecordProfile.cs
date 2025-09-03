using AutoMapper;
using Domain.Entities;
using DTOs.COM.COM_FeedbackRecord.Requests;
using DTOs.COM.COM_FeedbackRecord.Responses;

namespace Application.Mappers.COM
{
	public class COM_FeedbackRecordProfile : Profile
	{
		public COM_FeedbackRecordProfile()
		{
			CreateMap<COM_FeedbackRecord, FeedbackRecordResponse>().ReverseMap();
			CreateMap<CreateFeedbackRecordRequest, COM_FeedbackRecord>();
			CreateMap<UpdateFeedbackRecordRequest, COM_FeedbackRecord>();
		}
	}
}



