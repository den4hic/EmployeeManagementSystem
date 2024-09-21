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

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : CRUDRepositoryBase<Employee, EmployeeDto, EmployeeManagementSystemDbContext, int>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
