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
        Task<IEnumerable<UserDto>> GetUsersWithDetailsAsync();
        Task<(IEnumerable<UserDto>, int)> GetUsersWithDetailsFilteredAsync(int page, int pageSize, string sortField, string sortDirection, string filter, string role);
        Task<(int, int, int)> GetUsersStatisticsAsync();
    }
}
