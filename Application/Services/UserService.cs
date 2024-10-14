using Application.Abstractions;
using Application.Common;
using Application.DTOs;
using Domain.Enum;
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

    public async Task<Result<UserDto>> CreateUserAsync(UserDto userDto)
    {
        try
        {
            var user = await _userRepository.CreateAsync(userDto);
            return Result<UserDto>.Success(user);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Failed to create user: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user != null
            ? Result<UserDto>.Success(user)
            : Result<UserDto>.Failure($"User with id {id} not found");
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            return Result<IEnumerable<UserDto>>.Success(users);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UserDto>>.Failure($"Failed to retrieve users: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateUserAsync(UserDto userDto)
    {
        try
        {
            userDto.AspNetUser = null;
            await _userRepository.UpdateAsync(userDto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to update user: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return Result<bool>.Failure($"User with id {id} not found");

            var aspnetUser = await _userManager.FindByIdAsync(user.AspNetUserId);
            if (aspnetUser != null)
                await _userManager.DeleteAsync(aspnetUser);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to delete user: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return Result<UserDto>.Failure($"User with username {username} not found");

        var userDto = await _userRepository.GetByAspNetUserIdDetailedAsync(user.Id);
        return userDto != null
            ? Result<UserDto>.Success(userDto)
            : Result<UserDto>.Failure($"UserDto for username {username} not found");
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsersWithDetailsAsync()
    {
        try
        {
            var users = await _userRepository.GetUsersWithDetailsAsync();
            return Result<IEnumerable<UserDto>>.Success(users);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UserDto>>.Failure($"Failed to retrieve users with details: {ex.Message}");
        }
    }

    public async Task<Result<(IEnumerable<UserDto>, int)>> GetUsersWithDetailsFilteredAsync(int page, int pageSize, string sortField, string sortDirection, string filter, string role)
    {
        try
        {
            sortField = ToCapital(sortField);
            var (users, totalItems) = await _userRepository.GetUsersWithDetailsFilteredAsync(page, pageSize, sortField, sortDirection, filter, role);
            foreach (var user in users)
            {
                var aspNetUser = await _userManager.FindByIdAsync(user.AspNetUserId);
                if (aspNetUser != null)
                {
                    var roles = await _userManager.GetRolesAsync(aspNetUser);
                    user.Role = roles.FirstOrDefault();
                }
            }
            return Result<(IEnumerable<UserDto>, int)>.Success((users, totalItems));
        }
        catch (Exception ex)
        {
            return Result<(IEnumerable<UserDto>, int)>.Failure($"Failed to retrieve filtered users: {ex.Message}");
        }
    }

    private static string ToCapital(string sortField)
    {
        return sortField.Length > 0
            ? char.ToUpper(sortField[0]) + sortField.Substring(1).ToLower()
            : string.Empty;
    }

    public async Task<Result<(int, int, int)>> GetUsersStatisticsAsync()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var totalAdmins = await _userManager.GetUsersInRoleAsync(UserRole.Admin.ToString());
            var blockedUsers = users.Count(u => u.IsBlocked);
            return Result<(int, int, int)>.Success((users.Count(), totalAdmins.Count, blockedUsers));
        }
        catch (Exception ex)
        {
            return Result<(int, int, int)>.Failure($"Failed to retrieve user statistics: {ex.Message}");
        }
    }

    public async Task<Result<UserDto>> GetUserByIdWithGroups(int id)
    {
        var userDto = await _userRepository.GetUserByIdWithGroups(id);

        return userDto != null
            ? Result<UserDto>.Success(userDto)
            : Result<UserDto>.Failure("User was not found");
    }
}