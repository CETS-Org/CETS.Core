using AutoMapper;
using Domain.Entities;
using DTOs.COM_Feedback.Requests;
using DTOs.COM_Feedback.Responses;

namespace Application.Mappers
{
	public class COM_FeedbackProfile : Profile
	{
		public COM_FeedbackProfile()
		{
			CreateMap<COM_Feedback, FeedbackResponse>().ReverseMap();
			CreateMap<CreateFeedbackRequest, COM_Feedback>();
			CreateMap<UpdateFeedbackRequest, COM_Feedback>();
		}
	}
}



