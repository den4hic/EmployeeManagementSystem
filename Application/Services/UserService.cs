using Application.Abstractions;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(IUserRepository userRepository, UserManager<IdentityUser> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<UserDto> CreateUserAsync(UserDto userDto)
    {
        return await _userRepository.CreateAsync(userDto);
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async System.Threading.Tasks.Task UpdateUserAsync(UserDto userDto)
    {
        await _userRepository.UpdateAsync(userDto);
    }

    public async System.Threading.Tasks.Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        var aspnetUser = await _userManager.FindByIdAsync(user.AspNetUserId);
        if (aspnetUser != null)
            await _userManager.DeleteAsync(aspnetUser);
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return null;
        }

        return await _userRepository.GetByAspNetUserIdAsync(user.Id);
    }

    public async Task<IEnumerable<UserDto>> GetUsersWithDetailsAsync()
    {
        return await _userRepository.GetUsersWithDetailsAsync();
    }

    public async Task<(IEnumerable<UserDto>, int)> GetUsersWithDetailsFilteredAsync(int page, int pageSize, string sortField, string sortDirection, string filter)
    {
        var users = await _userRepository.GetUsersWithDetailsAsync();

        if (!string.IsNullOrEmpty(filter))
        {
            users = users.Where(u => u.FirstName.Contains(filter) || u.LastName.Contains(filter) || u.Email.Contains(filter));
        }

        users = ApplySorting(users, sortField, sortDirection);

        var totalItems = users.Count();

        var items = users
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(u => u)
            .ToList();

        return (items, totalItems);
    }

    private IEnumerable<UserDto> ApplySorting(IEnumerable<UserDto> query, string sortField, string sortDirection)
    {
        switch (sortField.ToLower())
        {
            case "firstname":
                return sortDirection.ToLower() == "asc" ? query.OrderBy(u => u.FirstName) : query.OrderByDescending(u => u.FirstName);
            case "lastname":
                return sortDirection.ToLower() == "asc" ? query.OrderBy(u => u.LastName) : query.OrderByDescending(u => u.LastName);
            case "email":
                return sortDirection.ToLower() == "asc" ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email);
            case "phonenumber":
                return sortDirection.ToLower() == "asc" ? query.OrderBy(u => u.PhoneNumber) : query.OrderByDescending(u => u.PhoneNumber);
            case "id":
                return sortDirection.ToLower() == "asc" ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id);
            default:
                return sortDirection.ToLower() == "asc" ? query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id);
        }
    }

    public async Task<(int, int)> GetUsersStatisticsAsync()
    {
        var users = await _userRepository.GetAllAsync();

        var totalAdmins = await _userManager.GetUsersInRoleAsync("Admin");

        return (users.Count(), totalAdmins.Count());
    }
}
