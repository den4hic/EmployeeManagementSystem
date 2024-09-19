using Application.DTOs;
using AutoMapper;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class TaskRepository : CRUDRepositoryBase<Domain.Entities.Task, TaskDto, EmployeeManagementSystemDbContext, int>
{
    public TaskRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
