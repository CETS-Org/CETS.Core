using DTOs.COM.COM_Notification.Responses;

namespace Application.Interfaces.COM
{
    public interface INotificationEventPublisher
    {
        Task PublishNotificationAsync(NotificationResponse notification);
        Task PublishNotificationsAsync(IEnumerable<NotificationResponse> notifications);
    }
}
