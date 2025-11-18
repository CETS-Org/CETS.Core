using Domain.Entities.MongoDB;

namespace Domain.Interfaces.COM
{
    public interface ICOM_NotificationRepository
    {
        Task<IReadOnlyList<COM_Notification>> GetAllAsync();
        Task<IReadOnlyList<COM_Notification>> GetByUserAsync(string userId);
        Task<COM_Notification?> GetByIdAsync(string id);
        Task<COM_Notification> CreateAsync(COM_Notification document);
        Task<bool> UpdateAsync(COM_Notification document);
        Task DeleteAsync(string id);
    }
}
