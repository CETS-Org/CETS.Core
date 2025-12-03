using System;

namespace DTOs.Message.Requests
{
    /// <summary>
    /// Message request để tạo payment reminder notification
    /// Gửi từ Worker sang API để tạo thông báo cho user
    /// </summary>
    public class CreatePaymentReminderNotificationRequest
    {
        public Guid ClassReservationId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentEmail { get; set; } = null!;
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public DateOnly DueDate { get; set; }
        public int DaysUntilDue { get; set; }
        public decimal Amount { get; set; }
        public string CoursePackageName { get; set; } = null!;
        
        /// <summary>
        /// Loại thông báo: Email, SMS, InApp, All
        /// </summary>
        public NotificationType NotificationType { get; set; } = NotificationType.All;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    
    public enum NotificationType
    {
        Email = 1,
        SMS = 2,
        InApp = 3,
        All = 4
    }
}






