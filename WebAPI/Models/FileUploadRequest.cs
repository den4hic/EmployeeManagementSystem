namespace WebAPI.Models;

public class FileUploadRequest
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public IFormFile File { get; set; }
}
