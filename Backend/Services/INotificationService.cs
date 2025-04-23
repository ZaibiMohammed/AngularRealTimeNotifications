using NotificationsApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationsApi.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<bool> MarkAsReadAsync(Guid notificationId);
        Task<IEnumerable<Notification>> GetNotificationsByGroupAsync(string groupName);
    }
}
