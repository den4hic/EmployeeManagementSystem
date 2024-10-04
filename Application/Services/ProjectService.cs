using Application.Abstractions;
using Application.Common;
using Application.DTOs;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<ProjectDto>> CreateProjectAsync(ProjectCreateDto projectDto)
    {
        try
        {
            var project = await _projectRepository.CreateCustomAsync(projectDto);
            return Result<ProjectDto>.Success(project);
        }
        catch (Exception ex)
        {
            return Result<ProjectDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<ProjectDto>> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project != null
            ? Result<ProjectDto>.Success(project)
            : Result<ProjectDto>.Failure($"Project with id {id} not found");
    }

    public async Task<Result<IEnumerable<ProjectDto>>> GetAllProjectsAsync()
    {
        try
        {
            var projects = await _projectRepository.GetProjectsWithDetailsAsync();
            return Result<IEnumerable<ProjectDto>>.Success(projects);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProjectDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateProjectAsync(ProjectCreateDto projectDto)
    {
        try
        {
            await _projectRepository.UpdateCustomAsync(projectDto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteProjectAsync(int id)
    {
        try
        {
            await _projectRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }
}
