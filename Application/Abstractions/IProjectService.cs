using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto);
    Task<ProjectDto> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task UpdateProjectAsync(ProjectDto projectDto);
    Task DeleteProjectAsync(int id);
}
