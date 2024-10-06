using Application.DTOs;

namespace Application.Abstractions;

public interface ITaskRepository : ICRUDRepository<TaskDto, int>
{
    Task<IEnumerable<TaskDto>> GetTasksByProjectId(int projectId);
}
