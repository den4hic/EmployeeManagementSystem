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

        var createdUser = await _userService.CreateUserAsync(userDto);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _userService.UpdateUserAsync(userDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpGet("current/{username}")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(string username)
    {
        var user = await _userService.GetUserByUsernameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("details")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersWithDetails()
    {
        var users = await _userService.GetUsersWithDetailsAsync();
        return Ok(users);
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
        var (users, totalItems) = await _userService.GetUsersWithDetailsFilteredAsync(page, pageSize, sortField, sortDirection, filter, role);
        return Ok(new { items = users, totalItems = totalItems});
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<(int, int, int)>> GetUsersStatistics()
    {
        var (totalUsers, totalAdmins, blockedUsers) = await _userService.GetUsersStatisticsAsync();
        return Ok(new { totalUsers = totalUsers, activeAdmins = totalAdmins, blockedUsers = blockedUsers });
    }

    [HttpPut("block/{id}")]
    public async Task<ActionResult> BlockUser(int id, [FromBody] bool newIsBlocked)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.IsBlocked = newIsBlocked;
        await _userService.UpdateUserAsync(user);
        return NoContent();
    }



}