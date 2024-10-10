using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                project.ProjectManagers.Add(new ProjectManager
                {
                    Project = project,
                    Manager = manager
                });
            }
        }

        foreach (var employeeId in projectDto.EmployeeIds)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                project.ProjectEmployees.Add(new ProjectEmployee
                {
                    Project = project,
                    Employee = employee
                });
            }
        }

        foreach (var taskId in projectDto.TaskIds)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null)
            {
                project.Tasks.Add(task);
            }
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProjectDto>(project);
    }


    public async Task<IEnumerable<ProjectDto>> GetProjectsWithDetailsAsync()
    {
        var projects = await _context.Projects.Include(p => p.ProjectManagers)
            .ThenInclude(pm => pm.Manager)
            .Include(p => p.ProjectEmployees)
            .ThenInclude(pe => pe.Employee)
            .Include(p => p.Tasks)
            .ToListAsync();

        foreach (var project in projects)
        {
            foreach (var task in project.Tasks)
            {
                task.Project = null;
                if (task.AssignedToEmployee != null)
                {
                    task.AssignedToEmployee.Tasks = null;
                }
            }
        }

        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async System.Threading.Tasks.Task UpdateCustomAsync(ProjectCreateDto projectDto)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectManagers)
            .Include(p => p.ProjectEmployees)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectDto.Id);

        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {projectDto.Id} not found.");
        }

        project.Name = projectDto.Name;
        project.Description = projectDto.Description;
        project.StartDate = projectDto.StartDate;
        project.EndDate = projectDto.EndDate;
        project.StatusId = projectDto.StatusId;

        project.ProjectManagers.Clear();
        foreach (var managerId in projectDto.ManagerIds)
        {
            var manager = await _context.Managers.FindAsync(managerId);
            if (manager != null)
            {
                project.ProjectManagers.Add(new ProjectManager
                {
                    Project = project,
                    Manager = manager
                });
            }
        }

        var removedEmployeeIds = project.ProjectEmployees
            .Where(pe => !projectDto.EmployeeIds.Contains(pe.EmployeeId))
            .Select(pe => pe.EmployeeId)
            .ToList();
        
        project.ProjectEmployees.Clear();
        foreach (var employeeId in projectDto.EmployeeIds)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                project.ProjectEmployees.Add(new ProjectEmployee
                {
                    Project = project,
                    Employee = employee
                });
            }
        }

        project.Tasks.Clear();
        foreach (var taskId in projectDto.TaskIds)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task != null)
            {
                project.Tasks.Add(task);
            }
        }

        if (removedEmployeeIds.Any())
        {
            var tasksToUpdate = await _context.Tasks
                    .Where(t => t.ProjectId == project.Id && t.AssignedToEmployeeId.HasValue && removedEmployeeIds.Contains(t.AssignedToEmployeeId.Value))
                    .ToListAsync();


            foreach (var task in tasksToUpdate)
            {
                task.AssignedToEmployeeId = null;
            }
        }

        await _context.SaveChangesAsync();
    }


}
