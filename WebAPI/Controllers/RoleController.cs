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
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("create-default-roles")]
    public async Task<IActionResult> CreateDefaultRoles()
    {
        await _roleService.CreateDefaultRolesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var success = await _roleService.CreateRoleAsync(roleName);
        if (!success)
            return BadRequest("Role already exists");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleService.GetRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(string id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        if (role == null)
            return NotFound();

        return Ok(role);
    }

    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var success = await _roleService.DeleteRoleAsync(roleName);
        if (!success)
            return NotFound();

        return Ok();
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(string username, string role, [FromBody] EmployeeManagerRoleDto roleData)
    {
        await _roleService.AssignRoleAsync(username, role, roleData);
        return Ok(new { Message = "Role assigned and entity created successfully" });
    }
}
