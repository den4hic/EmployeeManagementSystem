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

    public async Task<IEnumerable<ManagerDto>> GetAllWithUsersAsync()
    {
        var managers = await _context.Managers.Include(e => e.User).ThenInclude(u => u.UserPhoto).ToListAsync();
        foreach (var manager in managers)
        {
            manager.User.Manager = null;
            if (manager.User.UserPhoto != null)
            {
                manager.User.UserPhoto.User = null;
            }
        }
        return _mapper.Map<IEnumerable<ManagerDto>>(managers);
    }

    public async Task<ManagerDto> GetByUserIdAsync(int userId)
    {
        var manager = await _context.Managers.FirstOrDefaultAsync(u => u.UserId == userId);

        return manager != null ? _mapper.Map<ManagerDto>(manager) : null;
    }
}
