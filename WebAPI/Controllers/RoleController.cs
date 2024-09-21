using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IManagerRepository _managerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RoleController(
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager,
        IManagerRepository managerRepository,
        IEmployeeRepository employeeRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _managerRepository = managerRepository;
        _employeeRepository = employeeRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost("create-default-roles")]
    public async Task<IActionResult> CreateDefaultRoles()
    {
        string[] roleNames = { "User", "Admin", "Employee", "Manager" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (roleExist)
        {
            return BadRequest("Role already exists");
        }

        await _roleManager.CreateAsync(new IdentityRole(roleName));

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role);
    }

    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
        {
            return NotFound();
        }

        await _roleManager.DeleteAsync(role);

        return Ok();
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(string username, string role, [FromBody] EmployeeManagerRoleDto roleData)
    {
        var userIdentity = await _userManager.FindByNameAsync(username);
        if (userIdentity == null)
            return NotFound(new { Message = "User not found" });

        var roleExist = await _roleManager.RoleExistsAsync(role);
        if (!roleExist)
            return BadRequest(new { Message = "Role does not exist" });
        
        var result = await _userManager.AddToRoleAsync(userIdentity, role);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var user = await _userRepository.GetByAspNetUserId(userIdentity.Id);

        if (role == "Manager")
        {
            var managerDto = _mapper.Map<ManagerDto>(roleData);

            managerDto.UserId = user.Id;

            await _managerRepository.CreateAsync(managerDto);
        }
        else if (role == "Employee")
        {
            var employeeDto = _mapper.Map<EmployeeDto>(roleData);

            employeeDto.UserId = user.Id;

            await _employeeRepository.CreateAsync(employeeDto);
        }

        return Ok(new { Message = "Role assigned and entity created successfully" });
    }
}
