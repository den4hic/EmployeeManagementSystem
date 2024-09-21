using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(TaskDto taskDto);
    Task<TaskDto> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task UpdateTaskAsync(TaskDto taskDto);
    Task DeleteTaskAsync(int id);
}
