using Domain.Common;

namespace Domain.Entities;

public class Notification : IEntity<int>
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int Type { get; set; }

    public virtual NotificationGroup Group { get; set; } = null!;
}
