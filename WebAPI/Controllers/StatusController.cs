using Application.DTOs;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly ICRUDRepository<StatusDto, int> _statusRepository;

    public StatusController(ICRUDRepository<StatusDto, int> statusRepository)
    {
        _statusRepository = statusRepository;
    }

    [HttpPost]
    public async Task<ActionResult<StatusDto>> CreateStatus(StatusDto statusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdStatus = await _statusRepository.CreateAsync(statusDto);

        return CreatedAtAction(nameof(GetStatus), new { id = createdStatus.Id }, createdStatus);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetStatus(int id)
    {
        var status = await _statusRepository.GetByIdAsync(id);

        if (status == null)
        {
            return NotFound();
        }

        return Ok(status);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllStatuses()
    {
        var statuses = await _statusRepository.GetAllAsync();

        return Ok(statuses);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateStatus(StatusDto statusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _statusRepository.UpdateAsync(statusDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStatus(int id)
    {
        await _statusRepository.DeleteAsync(id);

        return NoContent();
    }
}
