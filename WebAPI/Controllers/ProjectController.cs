using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly EmployeeManagementSystemDbContext _context;
    private readonly IMapper _mapper;

    public ProjectController(EmployeeManagementSystemDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(ProjectDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = _mapper.Map<Project>(projectDto);

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var createdProjectDto = _mapper.Map<ProjectDto>(project);

        return CreatedAtAction(nameof(GetProject), new { id = createdProjectDto.Id }, createdProjectDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        var projectDto = _mapper.Map<ProjectDto>(project);

        return Ok(projectDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
    {
        var projects = await _context.Projects
            .Include(p => p.Tasks)
            .ToListAsync();

        var projectDtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);

        return Ok(projectDtos);
    }
}
