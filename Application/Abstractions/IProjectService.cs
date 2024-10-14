using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface IProjectService
{
    Task<Result<ProjectDto>> CreateProjectAsync(ProjectCreateDto projectDto);
    Task<Result<ProjectDto>> GetProjectByIdAsync(int id);
    Task<Result<IEnumerable<ProjectDto>>> GetAllProjectsAsync();
    Task<Result<bool>> UpdateProjectAsync(ProjectCreateDto projectDto);
    Task<Result<bool>> DeleteProjectAsync(int id);
    Task<Result<ProjectDto>> GetProjectByIdWithDetailsAsync(int id);
}
