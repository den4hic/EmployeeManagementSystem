namespace Domain.Entities;

public class UserNotificationGroups
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int GroupId { get; set; }
    public NotificationGroup Group { get; set; } = null!;
}
