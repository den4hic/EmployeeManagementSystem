using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : CRUDRepositoryBase<Employee, EmployeeDto, EmployeeManagementSystemDbContext, int>
    {
        public EmployeeRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
