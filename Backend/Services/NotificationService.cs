using Microsoft.AspNetCore.SignalR;
using NotificationsApi.Hubs;
using NotificationsApi.Models;
using System.Collections.Concurrent;

namespace NotificationsApi.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;
        private readonly ConcurrentDictionary<Guid, Notification> _notifications = new();

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return _notifications.Values.OrderByDescending(n => n.Timestamp);
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            if (notification.Id == Guid.Empty)
            {
                notification.Id = Guid.NewGuid();
            }
            
            notification.Timestamp = DateTime.UtcNow;
            
            _notifications[notification.Id] = notification;
            _logger.LogInformation($"Created notification: {notification.Id} - {notification.Message}");

            // Send to all clients if no target group is specified
            if (string.IsNullOrEmpty(notification.TargetGroup))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
            }
            else
            {
                // Send only to the specified group
                await _hubContext.Clients.Group(notification.TargetGroup)
                    .SendAsync("ReceiveNotification", notification);
            }

            return notification;
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId)
        {
            if (!_notifications.TryGetValue(notificationId, out var notification))
            {
                _logger.LogWarning($"Notification not found: {notificationId}");
                return false;
            }

            notification.IsRead = true;
            _logger.LogInformation($"Marked notification as read: {notificationId}");
            return true;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByGroupAsync(string groupName)
        {
            return _notifications.Values
                .Where(n => n.TargetGroup == groupName)
                .OrderByDescending(n => n.Timestamp);
        }
    }
}
