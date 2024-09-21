using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Infrastructure.Repositories;

public class ManagerRepository : CRUDRepositoryBase<Manager, ManagerDto, EmployeeManagementSystemDbContext, int>, IManagerRepository
{
    public ManagerRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
