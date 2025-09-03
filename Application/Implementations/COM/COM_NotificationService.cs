using Application.Interfaces.COM;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.COM;
using DTOs.COM.COM_Notification.Requests;
using DTOs.COM.COM_Notification.Responses;

namespace Application.Implementations.COM
{
	public class COM_NotificationService : BaseService<COM_Notification, NotificationResponse, UpdateNotificationRequest, CreateNotificationRequest>, ICOM_NotificationService
	{
		public COM_NotificationService(ICOM_NotificationRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
			: base(repository, unitOfWork, mapper)
		{
		}
	}
}



