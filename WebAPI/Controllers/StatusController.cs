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

        var result = await _statusService.CreateStatusAsync(statusDto);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetStatus), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StatusDto>> GetStatus(int id)
    {
        var result = await _statusService.GetStatusByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusDto>>> GetAllStatuses()
    {
        var result = await _statusService.GetAllStatusesAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateStatus(StatusDto statusDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _statusService.UpdateStatusAsync(statusDto);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStatus(int id)
    {
        var result = await _statusService.DeleteStatusAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}