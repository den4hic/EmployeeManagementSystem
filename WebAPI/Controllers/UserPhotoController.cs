using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserPhotoController : ControllerBase
{
    private readonly IUserPhotoService _userPhotoService;

    public UserPhotoController(IUserPhotoService userPhotoService)
    {
        _userPhotoService = userPhotoService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUserPhoto([FromForm] FileUploadRequest model)
    {
        IFormFile file = model.File;
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);

        var userPhotoDto = new UserPhotoDto
        {
            UserId = model.UserId,
            PhotoData = memoryStream.ToArray(),
            ContentType = file.ContentType,
            UploadDate = DateTime.Now
        };

        var result = await _userPhotoService.CreateAsync(userPhotoDto);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetUserPhoto), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserPhoto(int id)
    {
        var result = await _userPhotoService.GetUserPhotoById(id);

        if (result.IsSuccess)
        {
            return File(result.Value.PhotoData, result.Value.ContentType);
        }

        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserPhotos()
    {
        var result = await _userPhotoService.GetAllUserPhotosAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserPhoto(int id)
    {
        var result = await _userPhotoService.DeleteUserPhotoAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }
}
