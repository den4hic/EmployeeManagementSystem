using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class StatusRepository : CRUDRepositoryBase<Status, StatusDto, EmployeeManagementSystemDbContext, int>
{
    public StatusRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
