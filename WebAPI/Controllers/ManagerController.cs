using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Infrastructure.Context;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManagerController : ControllerBase
{
    private readonly EmployeeManagementSystemDbContext _context;
    private readonly IMapper _mapper;

    public ManagerController(EmployeeManagementSystemDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ManagerDto>> CreateManager(ManagerDto managerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var manager = _mapper.Map<Manager>(managerDto);

        _context.Managers.Add(manager);
        await _context.SaveChangesAsync();

        var createdManagerDto = _mapper.Map<ManagerDto>(manager);

        return CreatedAtAction(nameof(GetManager), new { id = createdManagerDto.Id }, createdManagerDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ManagerDto>> GetManager(int id)
    {
        var manager = await _context.Managers
            .Include(m => m.Employees)
            .Include(m => m.Projects)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (manager == null)
        {
            return NotFound();
        }

        var managerDto = _mapper.Map<ManagerDto>(manager);

        return Ok(managerDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ManagerDto>>> GetAllManagers()
    {
        var managers = await _context.Managers
            .Include(m => m.Employees)
            .Include(m => m.Projects)
            .ToListAsync();

        var managerDtos = _mapper.Map<IEnumerable<ManagerDto>>(managers);

        return Ok(managerDtos);
    }
}