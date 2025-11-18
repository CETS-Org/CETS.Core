using DTOs.COM.COM_Notification.Requests;
using DTOs.COM.COM_Notification.Responses;

namespace Application.Interfaces.COM
{
    public interface ICOM_NotificationService
    {
        Task<IReadOnlyList<NotificationResponse>> GetAllAsync();
        Task<IReadOnlyList<NotificationResponse>> GetByUserAsync(string userId);
        Task<NotificationResponse?> GetByIdAsync(string id);
        Task<NotificationResponse> CreateAsync(CreateNotificationRequest request);
        Task<NotificationResponse?> UpdateAsync(string id, UpdateNotificationRequest request);
        Task DeleteAsync(string id);
        Task<NotificationResponse?> MarkAsReadAsync(string id);
        Task<int> MarkAllAsReadAsync(string userId);
        Task<IReadOnlyList<NotificationResponse>> CreateManyAsync(IEnumerable<CreateNotificationRequest> requests);
    }
}
