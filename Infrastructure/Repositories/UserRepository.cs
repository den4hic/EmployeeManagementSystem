using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : CRUDRepositoryBase<User, UserDto, EmployeeManagementSystemDbContext, int>, IUserRepository
{
    public UserRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<UserDto> GetByAspNetUserIdAsync(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.AspNetUserId == id);

        return user != null ? _mapper.Map<UserDto>(user) : null;
    }
}
