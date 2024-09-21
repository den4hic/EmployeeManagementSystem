using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Abstractions;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly IManagerService _managerService;

    public ManagerController(IManagerService managerService)
    {
        _managerService = managerService;
    }

    [HttpPost]
    public async Task<ActionResult<ManagerDto>> CreateManager(ManagerDto managerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdManager = await _managerService.CreateManagerAsync(managerDto);

        return CreatedAtAction(nameof(GetManager), new { id = createdManager.Id }, createdManager);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ManagerDto>> GetManager(int id)
    {
        var manager = await _managerService.GetManagerByIdAsync(id);

        if (manager == null)
        {
            return NotFound();
        }

        return Ok(manager);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ManagerDto>>> GetAllManagers()
    {
        var managers = await _managerService.GetAllManagersAsync();

        return Ok(managers);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateManager(ManagerDto managerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _managerService.UpdateManagerAsync(managerDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteManager(int id)
    {
        var manager = await _managerService.GetManagerByIdAsync(id);

        if (manager == null)
        {
            return NotFound();
        }

        await _managerService.DeleteManagerAsync(id);

        return NoContent();
    }
}