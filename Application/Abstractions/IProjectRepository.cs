using Application.DTOs;
using Domain.Entities;

namespace Application.Abstractions;

public interface IProjectRepository : ICRUDRepository<ProjectDto, int>
{
    Task<ProjectDto> CreateCustomAsync(ProjectCreateDto projectDto);
}
