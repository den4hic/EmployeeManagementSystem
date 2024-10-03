using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.CreateUserAsync(userDto);
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetUser), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.UpdateUserAsync(userDto);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }

    [HttpGet("current/{username}")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(string username)
    {
        var result = await _userService.GetUserByUsernameAsync(username);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Error);
    }

    [HttpGet("details")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersWithDetails()
    {
        var result = await _userService.GetUsersWithDetailsAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("details/filtered")]
    public async Task<ActionResult<(IEnumerable<UserDto>, int)>> GetUsersWithDetailsFiltered(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortField = "id",
        [FromQuery] string sortDirection = "asc",
        [FromQuery] string filter = "",
        [FromQuery] string role = "")
    {
        var result = await _userService.GetUsersWithDetailsFilteredAsync(page, pageSize, sortField, sortDirection, filter, role);
        if (result.IsSuccess)
        {
            return Ok(new { items = result.Value.Item1, totalItems = result.Value.Item2 });
        }

        return BadRequest(result.Error);
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<(int, int, int)>> GetUsersStatistics()
    {
        var result = await _userService.GetUsersStatisticsAsync();
        if (result.IsSuccess)
        {
            var (totalUsers, totalAdmins, blockedUsers) = result.Value;
            return Ok(new { totalUsers, activeAdmins = totalAdmins, blockedUsers });
        }

        return BadRequest(result.Error);
    }

    [HttpPut("block/{id}")]
    public async Task<ActionResult> BlockUser(int id, [FromBody] bool newIsBlocked)
    {
        var userResult = await _userService.GetUserByIdAsync(id);
        if (!userResult.IsSuccess)
        {
            return NotFound(userResult.Error);
        }

        var user = userResult.Value;
        user.IsBlocked = newIsBlocked;
        var updateResult = await _userService.UpdateUserAsync(user);
        if (updateResult.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(updateResult.Error);
    }
}