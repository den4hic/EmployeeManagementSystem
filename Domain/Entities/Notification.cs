﻿using Domain.Common;
using Domain.Enum;

namespace Domain.Entities;

public class Notification : IEntity<int>
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public NotificationType Type { get; set; }

    public virtual NotificationGroup Group { get; set; } = null!;
}
