using Application.Common;
using Domain.Entities;
using Domain.Enum;

namespace Application.DTOs;

public class NotificationDto : BaseDto<int>
{
    public int GroupId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public NotificationType Type { get; set; }

    public virtual NotificationGroupDto Group { get; set; } = null!;
}
