using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (ModelState.IsValid)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);

            if (existingUserByEmail != null)
            {
                return BadRequest(new { Message = "A user with this email already exists." });
            }

            var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);

            if (existingUserByUsername != null)
            {
                return BadRequest(new { Message = "A user with this username already exists." });
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                var userDto = _mapper.Map<UserDto>(model);

                userDto.AspNetUserId = user.Id;

                await _userRepository.CreateAsync(userDto);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { message = "User created successfully" });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }            

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(new { Message = "Logged in successfully" });
        }
        return BadRequest(ModelState);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { Message = "Logged out successfully" });
    }

    [HttpPost("default-admin")]
    public async Task<ActionResult> CreateDefaultAdmin()
    {
        string adminUsername = "admin";
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newAdmin = new IdentityUser { UserName = adminUsername, Email = adminEmail };
            var result = await _userManager.CreateAsync(newAdmin, adminPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newAdmin, "Admin");

                var userDto = new UserDto
                {
                    FirstName = adminUsername,
                    LastName = adminUsername,
                    Email = adminEmail,
                    PhoneNumber = "1234567890"
                };

                userDto.AspNetUserId = newAdmin.Id;

                await _userRepository.CreateAsync(userDto);
            }
        }

        return NoContent();
    }
}
