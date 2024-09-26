using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpPost]
    public async Task<ActionResult<StatusDto>> CreateStatus(StatusDto statusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdStatus = await _statusService.CreateStatusAsync(statusDto);
        return CreatedAtAction(nameof(GetStatus), new { id = createdStatus.Id }, createdStatus);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StatusDto>> GetStatus(int id)
    {
        var status = await _statusService.GetStatusByIdAsync(id);

        if (status == null)
        {
            return NotFound();
        }

        return Ok(status);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusDto>>> GetAllStatuses()
    {
        var statuses = await _statusService.GetAllStatusesAsync();
        return Ok(statuses);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateStatus(StatusDto statusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _statusService.UpdateStatusAsync(statusDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStatus(int id)
    {
        await _statusService.DeleteStatusAsync(id);
        return NoContent();
    }
}