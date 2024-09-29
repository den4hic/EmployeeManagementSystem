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
        var project = _mapper.Map<Project>(projectDto);

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

        foreach (var managerId in projectDto.EmployeeIds)
        {
            var employee = await _context.Employees.FindAsync(managerId);
            if (employee != null)
            {
                var projectEmployee = new ProjectEmployee
                {
                    Project = project,
                    Employee = employee
                };
                project.ProjectEmployees.Add(projectEmployee);
            }
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(project);
    }
}
