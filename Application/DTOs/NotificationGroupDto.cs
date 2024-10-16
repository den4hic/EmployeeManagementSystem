using Application.Common;
using Domain.Entities;

namespace Application.DTOs;

public class NotificationGroupDto : BaseDto<int>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<NotificationDto> Notifications { get; set; } = new List<NotificationDto>();

    public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
}
