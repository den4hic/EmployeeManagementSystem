using Application.DTOs;

namespace Application.Abstractions;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(TaskDto taskDto);
    Task<TaskDto> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task UpdateTaskAsync(TaskDto taskDto);
    Task DeleteTaskAsync(int id);
}
