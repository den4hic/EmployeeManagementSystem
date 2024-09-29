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

    public async Task UpdateUserAsync(UserDto userDto)
    {
        await _userRepository.UpdateAsync(userDto);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
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
}
