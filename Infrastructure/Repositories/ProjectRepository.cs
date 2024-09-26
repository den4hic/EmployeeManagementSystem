using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class ProjectRepository : CRUDRepositoryBase<Project, ProjectDto, EmployeeManagementSystemDbContext, int>, IProjectRepository
{
    public ProjectRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<ProjectDto> CreateCustomAsync(ProjectCreateDto projectDto)
    {
        var project = new Project
        {
            Name = projectDto.Name,
            Description = projectDto.Description,
            StartDate = projectDto.StartDate,
            EndDate = projectDto.EndDate,
            StatusId = projectDto.StatusId
        };

        // Додаємо зв'язок із існуючими менеджерами через ProjectManager
        foreach (var managerId in projectDto.ManagerIds)
        {
            var manager = await _context.Managers.FindAsync(managerId);
            if (manager != null)
            {
                var projectManager = new ProjectManager
                {
                    Project = project,
                    Manager = manager
                };
                project.ProjectManagers.Add(projectManager);
            }
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }
}
