using Domain.Common;
using Domain.Enum;

namespace Domain.Entities;

public class Notification : IEntity<int>
{
    public int Id { get; set; }

    public int? GroupId { get; set; }

    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; }

    public NotificationType Type { get; set; }

    public int? ReceiverId { get; set; }

    public int? SenderId { get; set; }

    public virtual NotificationGroup? Group { get; set; } = null!;

    public virtual User? Receiver { get; set; }

    public virtual User? Sender { get; set; }

    public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
}
