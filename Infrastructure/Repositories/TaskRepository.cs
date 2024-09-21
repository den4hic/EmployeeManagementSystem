using Application.DTOs;
using AutoMapper;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Infrastructure.Repositories;

public class TaskRepository : CRUDRepositoryBase<Domain.Entities.Task, TaskDto, EmployeeManagementSystemDbContext, int>, ITaskRepository
{
    public TaskRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
