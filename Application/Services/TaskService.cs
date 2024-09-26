using Application.Abstractions;
using Application.DTOs;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
    {
        return await _taskRepository.CreateAsync(taskDto);
    }

    public async Task<TaskDto> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        return await _taskRepository.GetAllAsync();
    }

    public async Task UpdateTaskAsync(TaskDto taskDto)
    {
        await _taskRepository.UpdateAsync(taskDto);
    }

    public async Task DeleteTaskAsync(int id)
    {
        await _taskRepository.DeleteAsync(id);
    }
}
