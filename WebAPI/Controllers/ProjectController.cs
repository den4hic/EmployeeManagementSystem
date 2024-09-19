using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ICRUDRepository<ProjectDto, int> _projectRepository;

    public ProjectController(ICRUDRepository<ProjectDto, int> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(ProjectDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdProject = await _projectRepository.CreateAsync(projectDto);

        return CreatedAtAction(nameof(GetProject), new { id = createdProject.Id }, createdProject);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProject(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllProjects()
    {
        var projects = await _projectRepository.GetAllAsync();

        return Ok(projects);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProject(ProjectDto projectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _projectRepository.UpdateAsync(projectDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(int id)
    {
        await _projectRepository.DeleteAsync(id);

        return NoContent();
    }
}
