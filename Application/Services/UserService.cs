using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;

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

    public async Task<(IEnumerable<UserDto>, int)> GetUsersWithDetailsFilteredAsync(int page, int pageSize, string sortField, string sortDirection, string filter, string role)
    {
        sortField = ToCapital(sortField);

        var (users, totalItems) = await _userRepository.GetUsersWithDetailsFilteredAsync(page, pageSize, sortField, sortDirection, filter, role);

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.AspNetUserId));

            user.Role = roles.FirstOrDefault();
        }

        return (users, totalItems);
    }

    private static string ToCapital(string sortField)
    {
        sortField = sortField.ToUpper()[0] + sortField.ToLower().Substring(1);
        return sortField;
    }

    public async Task<(int, int)> GetUsersStatisticsAsync()
    {
        var users = await _userRepository.GetAllAsync();

        var totalAdmins = await _userManager.GetUsersInRoleAsync("Admin");

        return (users.Count(), totalAdmins.Count());
    }
}
