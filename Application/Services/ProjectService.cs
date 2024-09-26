using Application.Abstractions;
using Application.DTOs;
using Domain.Entities;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDto> CreateProjectAsync(ProjectCreateDto projectDto)
    {

        return await _projectRepository.CreateCustomAsync(projectDto);
    }

    public async Task<ProjectDto> GetProjectByIdAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllAsync();
    }

    public async System.Threading.Tasks.Task UpdateProjectAsync(ProjectDto projectDto)
    {
        await _projectRepository.UpdateAsync(projectDto);
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(int id)
    {
        await _projectRepository.DeleteAsync(id);
    }
}
