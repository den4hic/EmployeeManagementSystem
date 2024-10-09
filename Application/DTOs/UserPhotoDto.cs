using Application.Common;

namespace Application.DTOs;

public class UserPhotoDto : BaseDto<int>
{
    public int UserId { get; set; }
    public string ContentType { get; set; }
    public DateTime UploadDate { get; set; }
    public byte[] PhotoData { get; set; }
}
