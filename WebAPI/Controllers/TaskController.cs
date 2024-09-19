using Application.DTOs;
using AutoMapper;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ICRUDRepository<TaskDto, int> _taskRepository;

    public TaskController(ICRUDRepository<TaskDto, int> taskRepository)
    {
        _taskRepository = taskRepository;
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(TaskDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdTask = await _taskRepository.CreateAsync(taskDto);

        return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTask(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return Ok(task);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllTasks()
    {
        var tasks = await _taskRepository.GetAllAsync();

        return Ok(tasks);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateTask(TaskDto taskDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _taskRepository.UpdateAsync(taskDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        await _taskRepository.DeleteAsync(id);

        return NoContent();
    }
}
