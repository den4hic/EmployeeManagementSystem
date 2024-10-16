using Application.Common;
using Domain.Entities;
using Domain.Enum;

namespace Application.DTOs;

public class NotificationDto : BaseDto<int>
{
    public int? GroupId { get; set; }

    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; }

    public NotificationType Type { get; set; }

    public int? ReceiverId { get; set; }

    public int? SenderId { get; set; }

    public virtual NotificationGroup? Group { get; set; } = null!;

    public virtual User? Receiver { get; set; }

    public virtual User? Sender { get; set; }

    public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
}
