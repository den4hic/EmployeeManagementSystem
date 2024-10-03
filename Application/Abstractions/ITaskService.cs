using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface ITaskService
{
    Task<Result<TaskDto>> CreateTaskAsync(TaskDto taskDto);
    Task<Result<TaskDto>> GetTaskByIdAsync(int id);
    Task<Result<IEnumerable<TaskDto>>> GetAllTasksAsync();
    Task<Result<bool>> UpdateTaskAsync(TaskDto taskDto);
    Task<Result<bool>> DeleteTaskAsync(int id);
}
