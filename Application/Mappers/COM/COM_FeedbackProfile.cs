using AutoMapper;
using Domain.Entities;
using DTOs.COM.COM_Feedback.Requests;
using DTOs.COM.COM_Feedback.Responses;

namespace Application.Mappers.COM
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



