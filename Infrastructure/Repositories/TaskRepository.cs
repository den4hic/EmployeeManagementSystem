using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskRepository : CRUDRepositoryBase<Domain.Entities.Task, TaskDto, EmployeeManagementSystemDbContext, int>, ITaskRepository
{
    public TaskRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public Task<IEnumerable<TaskDto>> GetTasksByProjectId(int projectId)
    {
        var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).Include(t => t.AssignedToEmployee).ThenInclude(e => e.User);
        foreach (var task in tasks)
        {
            if (task.AssignedToEmployee != null)
            {
                task.AssignedToEmployee.Tasks = null;
                task.AssignedToEmployee.User.Employee = null;
            }
                
        }
        return Task.FromResult(_mapper.Map<IEnumerable<TaskDto>>(tasks));
    }
}
