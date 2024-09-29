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

    public async Task<IEnumerable<UserDto>> GetUsersWithDetailsAsync()
    {
        var users = await _context.Users.Include(u => u.AspNetUser).Include(u => u.Employee).Include(u => u.Manager).ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users) {
            var userDto = _mapper.Map<UserDto>(user);
            userDtos.Add(userDto);
        }

        return userDtos;
    }
}
