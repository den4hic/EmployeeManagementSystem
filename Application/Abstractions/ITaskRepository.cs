using Application.DTOs;

namespace Application.Abstractions;

public interface ITaskRepository : ICRUDRepository<TaskDto, int>
{
}
