using Application.Abstractions;
using Application.Common;
using Application.DTOs;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<TaskDto>> CreateTaskAsync(TaskDto taskDto)
    {
        try
        {
            var task = await _taskRepository.CreateAsync(taskDto);
            return Result<TaskDto>.Success(task);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure($"Failed to create task: {ex.Message}");
        }
    }

    public async Task<Result<TaskDto>> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null
            ? Result<TaskDto>.Success(task)
            : Result<TaskDto>.Failure($"Task with id {id} not found");
    }

    public async Task<Result<IEnumerable<TaskDto>>> GetAllTasksAsync()
    {
        try
        {
            var tasks = await _taskRepository.GetAllAsync();
            return Result<IEnumerable<TaskDto>>.Success(tasks);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TaskDto>>.Failure($"Failed to retrieve tasks: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<TaskDto>>> GetTasksByProjectId(int projectId)
    {
        try
        {
            var tasks = await _taskRepository.GetTasksByProjectId(projectId);
            return Result<IEnumerable<TaskDto>>.Success(tasks);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TaskDto>>.Failure($"Failed to retrieve tasks: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateTaskAsync(TaskDto taskDto)
    {
        try
        {
            await _taskRepository.UpdateAsync(taskDto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to update task: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteTaskAsync(int id)
    {
        try
        {
            await _taskRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to delete task: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateTaskStatusAsync(int taskId, int statusId)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                return Result<bool>.Failure($"Task with id {taskId} not found");
            }

            task.StatusId = statusId;
            await _taskRepository.UpdateAsync(task);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to update task status: {ex.Message}");
        }
    }
}
