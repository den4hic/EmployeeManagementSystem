using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new { Errors = errors });
        }

        var result = await _accountService.RegisterAsync(model);
        if (result.IsSuccess)
        {
            return Ok(new { message = "User created successfully" });
        }
        return BadRequest(new { Errors = new List<string> { result.Error } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var loginResult = await _accountService.LoginAsync(model);
        if (loginResult.IsSuccess)
        {
            var tokenResult = await _accountService.GenerateJwtTokenAsync(model, true);
            if (tokenResult.IsSuccess)
            {
                return Ok(tokenResult.Value);
            }
            return BadRequest(new { Error = tokenResult.Error });
        }
        return Unauthorized(loginResult.Error);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var username = User.FindFirstValue("username");

        var result = await _accountService.LogoutAsync(username);
        if (result.IsSuccess)
        {
            return Ok(new { message = "Logged out successfully" });
        }
        return BadRequest(new { Error = result.Error });
    }

    [HttpPost("default-admin")]
    public async Task<ActionResult> CreateDefaultAdmin()
    {
        var result = await _accountService.CreateDefaultAdminAsync();
        if (result.IsSuccess)
        {
            return NoContent();
        }
        return BadRequest(new { message = result.Error });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
    {
        var result = await _accountService.RefreshToken(tokenDto);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(new { Error = result.Error });
    }
}