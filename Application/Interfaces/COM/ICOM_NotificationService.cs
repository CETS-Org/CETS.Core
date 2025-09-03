using Domain.Entities;
using DTOs.COM.COM_Notification.Requests;
using DTOs.COM.COM_Notification.Responses;

namespace Application.Interfaces.COM
{
	public interface ICOM_NotificationService : IBaseService<COM_Notification, NotificationResponse, UpdateNotificationRequest, CreateNotificationRequest>
	{
	}
}



