using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Context;
using Infrastructure.Abstractions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IManagerRepository _managerRepository;

    public ManagerController(IManagerRepository managerRepository)
    {
        _managerRepository = managerRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ManagerDto>> CreateManager(ManagerDto managerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdManager = await _managerRepository.CreateAsync(managerDto);

        return CreatedAtAction(nameof(GetManager), new { id = createdManager.Id }, createdManager);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetManager(int id)
    {
        var manager = await _managerRepository.GetByIdAsync(id);

        if (manager == null)
        {
            return NotFound();
        }

        return Ok(manager);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllManagers()
    {
        var managers = await _managerRepository.GetAllAsync();

        return Ok(managers);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateManager(ManagerDto managerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _managerRepository.UpdateAsync(managerDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteManager(int id)
    {
        var manager = await _managerRepository.GetByIdAsync(id);

        if (manager == null)
        {
            return NotFound();
        }

        await _managerRepository.DeleteAsync(id);

        return NoContent();
    }
}