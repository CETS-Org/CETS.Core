using AutoMapper;
using Domain.Entities;
using DTOs.COM_Notification.Requests;
using DTOs.COM_Notification.Responses;

namespace Application.Mappers
{
	public class COM_NotificationProfile : Profile
	{
		public COM_NotificationProfile()
		{
			CreateMap<COM_Notification, NotificationResponse>().ReverseMap();
			CreateMap<CreateNotificationRequest, COM_Notification>();
			CreateMap<UpdateNotificationRequest, COM_Notification>();
		}
	}
}



