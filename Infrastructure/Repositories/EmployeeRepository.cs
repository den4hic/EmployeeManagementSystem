using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : CRUDRepositoryBase<Employee, EmployeeDto, EmployeeManagementSystemDbContext, int>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<EmployeeDto> GetByUserIdAsync(int userId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(u => u.UserId == userId);

            return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
        }
    }
}
