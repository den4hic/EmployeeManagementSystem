using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public partial class User : IEntity<int>
{
    public int Id { get; set; }

    public string AspNetUserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? RefreshToken { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual IdentityUser AspNetUser { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual UserPhoto? UserPhoto { get; set; }

    public virtual ICollection<UserNotificationGroups> UserNotificationGroups { get; set; } = new List<UserNotificationGroups>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();
}
