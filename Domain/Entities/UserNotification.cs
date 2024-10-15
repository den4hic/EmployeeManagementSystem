using Domain.Common;

namespace Domain.Entities;

public class UserNotification : IEntity<int>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int NotificationId { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public virtual Notification Notification { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
