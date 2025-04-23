using System;

namespace NotificationsApi.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; } = NotificationType.Info;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? TargetGroup { get; set; }
        public bool IsRead { get; set; } = false;
    }

    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error
    }
}
