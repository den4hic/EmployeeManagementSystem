using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        if (result)
        {
            return Ok(new { message = "User created successfully" });
        }

        return BadRequest(new { Errors = new List<string> { "User creation failed" } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _accountService.LoginAsync(model);

        if (result)
        {
            var token = await _accountService.GenereateJwtTokenAsync(model, true);
            return Ok(token);
        }


        return Unauthorized("Invalid credentials");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpPost("default-admin")]
    public async Task<ActionResult> CreateDefaultAdmin()
    {
        var result = await _accountService.CreateDefaultAdminAsync();

        if (result)
        {
            return NoContent();
        }

        return BadRequest(new { message = "Problems with default admin creation" });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
    {
        var token = await _accountService.RefreshToken(tokenDto);
        return Ok(token);
    }
}