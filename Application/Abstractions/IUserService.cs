using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface IUserService
{
    Task<Result<UserDto>> CreateUserAsync(UserDto userDto);
    Task<Result<UserDto>> GetUserByIdAsync(int id);
    Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
    Task<Result<bool>> UpdateUserAsync(UserDto userDto);
    Task<Result<bool>> DeleteUserAsync(int id);
    Task<Result<UserDto>> GetUserByUsernameAsync(string username);
    Task<Result<IEnumerable<UserDto>>> GetUsersWithDetailsAsync();
    Task<Result<(IEnumerable<UserDto>, int)>> GetUsersWithDetailsFilteredAsync(int page, int pageSize, string sortField, string sortDirection, string filter, string role);
    Task<Result<(int, int, int)>> GetUsersStatisticsAsync();
    Task<Result<UserDto>> GetUserByIdWithGroups(int id);
}
