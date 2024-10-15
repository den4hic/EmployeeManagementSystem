using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : CRUDRepositoryBase<User, UserDto, EmployeeManagementSystemDbContext, int>, IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(EmployeeManagementSystemDbContext context, IMapper mapper, UserManager<IdentityUser> userManager) : base(context, mapper)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> GetByAspNetUserIdAsync(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.AspNetUserId == id);

        if (user?.Employee != null)
        {
            user.Employee.User = null;
        }

        if (user?.Manager != null)
        {
            user.Manager.User = null;
        }

        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> GetByAspNetUserIdDetailedAsync(string id)
    {
        var user = await _context.Users.Include(u => u.Manager).Include(u => u.Employee).Include(u => u.UserPhoto).FirstOrDefaultAsync(u => u.AspNetUserId == id);

        if (user?.Employee != null)
        {
            user.Employee.User = null;
        }

        if (user?.Manager != null)
        {
            user.Manager.User = null;
        }

        if (user?.UserPhoto != null)
        {
            user.UserPhoto.User = null;
        }

        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto> GetUserByIdWithGroups(int id)
    {
        var user = await _context.Users.Include(u => u.UserNotificationGroups).ThenInclude(un => un.Group).FirstOrDefaultAsync(u => u.Id == id);

        return user != null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<IEnumerable<UserDto>> GetUsersWithDetailsAsync()
    {
        var users = await _context.Users.Include(u => u.AspNetUser).Include(u => u.Employee).Include(u => u.Manager).Include(u => u.UserPhoto).ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users) {
            if (user.Employee != null)
            {
                user.Employee.User = null;
            }
            if (user.Manager != null)
            {
                user.Manager.User = null;
            }
            if (user.UserPhoto != null)
            {
                user.UserPhoto.User = null;
            }
            var userDto = _mapper.Map<UserDto>(user);
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<(IEnumerable<UserDto>, int)> GetUsersWithDetailsFilteredAsync(int page, int pageSize, string sortField, string sortDirection, string filter, string role)
    {
        var query = _context.Users.Include(u => u.AspNetUser).Include(u => u.Employee).Include(u => u.Manager).Include(u => u.UserPhoto).AsQueryable();

        var totalItems = await query.CountAsync();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(u => u.FirstName.Contains(filter) || u.LastName.Contains(filter) || u.Email.Contains(filter));
        }

        query = sortDirection.ToLower() switch
        {
            "asc" => query.OrderBy(u => EF.Property<object>(u, sortField)),
            "desc" => query.OrderByDescending(u => EF.Property<object>(u, sortField)),
            _ => query
        };

        var users = new List<User>();

        if (!string.IsNullOrEmpty(role))
        {
            var filterRoleUser = query.AsEnumerable();

            filterRoleUser = filterRoleUser.Where(u => _userManager.GetRolesAsync(u.AspNetUser).Result.Contains(role));

            filterRoleUser = filterRoleUser.Skip(page * pageSize).Take(pageSize);

            users = filterRoleUser.ToList();
        } else
        {
            query = query.Skip(page * pageSize).Take(pageSize);

            users = await query.ToListAsync();
        }

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            if (user.Employee != null)
            {
                user.Employee.User = null;
            }
            if (user.Manager != null)
            {
                user.Manager.User = null;
            }
            if (user.UserPhoto != null)
            {
                user.UserPhoto.User = null;
            }
            var userDto = _mapper.Map<UserDto>(user);
            userDtos.Add(userDto);
        }

        return (userDtos, totalItems);
    }

    public async Task<UserDto> GetUsersWithGroupsAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.UserNotificationGroups)
            .ThenInclude(ung => ung.Group)
            .FirstOrDefaultAsync(u => u.Id == id);

        return _mapper.Map<UserDto>(user);
    }
}
