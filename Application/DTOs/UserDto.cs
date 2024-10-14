using Application.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.DTOs;

public class UserDto : BaseDto<int>
{
    public string AspNetUserId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public bool IsBlocked { get; set; }
    public string? Role { get; set; }
    public virtual IdentityUser? AspNetUser { get; set; } = null!;
    public virtual EmployeeDto? Employee { get; set; } = null!;
    public virtual ManagerDto? Manager { get; set; } = null!;
    public virtual UserPhotoDto? UserPhoto { get; set; } = null!;
    public virtual ICollection<NotificationGroupDto> NotificationGroups { get; set; } = new List<NotificationGroupDto>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
