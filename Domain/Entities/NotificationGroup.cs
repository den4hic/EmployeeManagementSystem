using Domain.Common;

namespace Domain.Entities;

public class NotificationGroup : IEntity<int>
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<UserNotificationGroups> UserNotificationGroups { get; set; } = new List<UserNotificationGroups>();
}
