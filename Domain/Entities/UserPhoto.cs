namespace Domain.Entities;

public class UserPhoto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public byte[] PhotoData { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public DateTime UploadDate { get; set; }

    public virtual User User { get; set; } = null!;
}
