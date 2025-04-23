using Microsoft.AspNetCore.Mvc;
using NotificationsApi.Models;
using NotificationsApi.Services;

namespace NotificationsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            INotificationService notificationService,
            ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        [HttpGet("group/{groupName}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return BadRequest("Group name cannot be empty");
            }

            var notifications = await _notificationService.GetNotificationsByGroupAsync(groupName);
            return Ok(notifications);
        }

        [HttpPost]
        public async Task<ActionResult<Notification>> CreateNotification(Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("Notification cannot be null");
            }

            var createdNotification = await _notificationService.CreateNotificationAsync(notification);
            return CreatedAtAction(nameof(GetAllNotifications), new { id = createdNotification.Id }, createdNotification);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            if (!result)
            {
                return NotFound($"Notification with ID {id} not found");
            }

            return NoContent();
        }
    }
}
