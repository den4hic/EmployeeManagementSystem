using Application.DTOs;

namespace Application.Abstractions;

public interface IProjectRepository : ICRUDRepository<ProjectDto, int>
{
}
