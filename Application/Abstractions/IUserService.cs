using Application.DTOs;

namespace Application.Abstractions
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task UpdateUserAsync(UserDto userDto);
        Task DeleteUserAsync(int id);
        Task<UserDto> GetUserByUsernameAsync(string username);
    }
}
