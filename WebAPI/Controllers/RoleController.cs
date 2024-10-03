using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("create-default-roles")]
    public async Task<IActionResult> CreateDefaultRoles()
    {
        var result = await _roleService.CreateDefaultRolesAsync();
        if (result.IsSuccess)
        {
            return Ok("Default roles created successfully");
        }
        return BadRequest(result.Error);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var result = await _roleService.CreateRoleAsync(roleName);
        if (result.IsSuccess)
        {
            return Ok("Role created successfully");
        }
        return BadRequest(result.Error);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _roleService.GetRolesAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRole(string id)
    {
        var result = await _roleService.GetRoleByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return NotFound(result.Error);
    }

    [HttpDelete("{roleName}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var result = await _roleService.DeleteRoleAsync(roleName);
        if (result.IsSuccess)
        {
            return Ok("Role deleted successfully");
        }
        return NotFound(result.Error);
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(int userId, string role, [FromBody] EmployeeManagerRoleDto roleData)
    {
        var result = await _roleService.AssignRoleAsync(userId, role, roleData);
        if (result.IsSuccess)
        {
            return Ok(new { Message = "Role assigned and entity created successfully" });
        }
        return BadRequest(result.Error);
    }
}