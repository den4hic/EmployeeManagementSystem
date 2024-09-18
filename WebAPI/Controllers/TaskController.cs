using Application.DTOs;
using AutoMapper;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly EmployeeManagementSystemDbContext _context;
    private readonly IMapper _mapper;

    public TaskController(EmployeeManagementSystemDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(TaskDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var task = _mapper.Map<Domain.Entities.Task>(taskDto);

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var createdTaskDto = _mapper.Map<TaskDto>(task);

        return CreatedAtAction(nameof(GetTask), new { id = createdTaskDto.Id }, createdTaskDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.Employee)
            .Include(t => t.Project)
            .Include(t => t.Status)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        var taskDto = _mapper.Map<TaskDto>(task);

        return Ok(taskDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
    {
        var tasks = await _context.Tasks
            .Include(t => t.Employee)
            .Include(t => t.Project)
            .Include(t => t.Status)
            .ToListAsync();

        var taskDtos = _mapper.Map<IEnumerable<TaskDto>>(tasks);

        return Ok(taskDtos);
    }
}
