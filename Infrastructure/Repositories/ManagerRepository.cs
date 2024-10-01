using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ManagerRepository : CRUDRepositoryBase<Manager, ManagerDto, EmployeeManagementSystemDbContext, int>, IManagerRepository
{
    public ManagerRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async System.Threading.Tasks.Task<ManagerDto> GetByUserIdAsync(int userId)
    {
        var manager = await _context.Managers.FirstOrDefaultAsync(u => u.UserId == userId);

        return manager != null ? _mapper.Map<ManagerDto>(manager) : null;
    }
}
