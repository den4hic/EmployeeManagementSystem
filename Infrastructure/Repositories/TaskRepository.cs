using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class TaskRepository : CRUDRepositoryBase<Domain.Entities.Task, TaskDto, EmployeeManagementSystemDbContext, int>, ITaskRepository
{
    public TaskRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public Task<IEnumerable<TaskDto>> GetTasksByProjectId(int projectId)
    {
        var tasks = _context.Tasks.Where(t => t.ProjectId == projectId);
        return Task.FromResult(_mapper.Map<IEnumerable<TaskDto>>(tasks));
    }
}
