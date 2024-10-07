using Application.DTOs;

namespace Application.Abstractions;

public interface IUserRepository : ICRUDRepository<UserDto, int>
{
    Task<UserDto> GetByAspNetUserIdAsync(string id);
    Task<UserDto> GetByAspNetUserIdDetailedAsync(string id);
    Task<IEnumerable<UserDto>> GetUsersWithDetailsAsync();
    Task<(IEnumerable<UserDto>, int)> GetUsersWithDetailsFilteredAsync(int page, int pageSize, string sortField, string sortDirection, string filter, string role);
}
