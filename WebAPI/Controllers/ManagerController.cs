﻿using Application.Abstractions;
using Application.DTOs;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IManagerService _managerService;

    public ManagerController(IManagerService managerService)
    {
        _managerService = managerService;
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPost]
    public async Task<ActionResult<ManagerDto>> CreateManager(ManagerDto managerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _managerService.CreateManagerAsync(managerDto);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetManager), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Employee)}")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ManagerDto>> GetManager(int id)
    {
        var manager = await _managerService.GetManagerByIdAsync(id);

        if (manager.IsSuccess)
        {
            return Ok(manager.Value);
        }

        return NotFound(manager.Error);
    }

    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)},{nameof(UserRole.Employee)}")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ManagerDto>>> GetAllManagers()
    {
        var managers = await _managerService.GetAllManagersAsync();

        if (managers.IsSuccess)
        {
            return Ok(managers.Value);
        }

        return BadRequest(managers.Error);
    }

    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Employee)}")]
    [HttpPut]
    public async Task<ActionResult> UpdateManager(ManagerDto managerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _managerService.UpdateManagerAsync(managerDto);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteManager(int id)
    {
        var result = await _managerService.DeleteManagerAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}