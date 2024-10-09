namespace WebAPI.Models;

public class FileUploadRequest
{
    public int UserId { get; set; }
    public IFormFile File { get; set; }
}
