using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] ProjectCreateDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _projectService.CreateProjectAsync(projectDto);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetProject), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project.IsSuccess)
        {
            return Ok(project.Value);
        }

        return NotFound(project.Error);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();

        if (projects.IsSuccess)
        {
            return Ok(projects.Value);
        }

        return BadRequest(projects.Error);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProject([FromBody] ProjectCreateDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _projectService.UpdateProjectAsync(projectDto);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}