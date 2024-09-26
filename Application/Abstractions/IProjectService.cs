using Application.DTOs;

namespace Application.Abstractions;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectDto);
    Task<ProjectDto> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task UpdateProjectAsync(ProjectDto projectDto);
    Task DeleteProjectAsync(int id);
}
