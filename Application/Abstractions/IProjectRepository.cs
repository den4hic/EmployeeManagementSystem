using Application.DTOs;

namespace Application.Abstractions;

public interface IProjectRepository : ICRUDRepository<ProjectDto, int>
{
    Task<ProjectDto> CreateCustomAsync(ProjectCreateDto projectDto);
    Task<IEnumerable<ProjectDto>> GetProjectsWithDetailsAsync();
    Task UpdateCustomAsync(ProjectCreateDto projectDto);
}
