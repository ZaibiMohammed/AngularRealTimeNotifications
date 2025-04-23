using Microsoft.AspNetCore.SignalR;
using NotificationsApi.Models;
using System.Threading.Tasks;

namespace NotificationsApi.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public async Task SendNotification(Notification notification)
        {
            _logger.LogInformation($"Sending notification: {notification.Message}");
            await Clients.All.SendAsync("ReceiveNotification", notification);
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation($"Client {Context.ConnectionId} joined group {groupName}");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation($"Client {Context.ConnectionId} left group {groupName}");
        }

        public async Task SendNotificationToGroup(string groupName, Notification notification)
        {
            _logger.LogInformation($"Sending notification to group {groupName}: {notification.Message}");
            await Clients.Group(groupName).SendAsync("ReceiveNotification", notification);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
