using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : CRUDRepositoryBase<Employee, EmployeeDto, EmployeeManagementSystemDbContext, int>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
