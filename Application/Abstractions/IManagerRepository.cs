using Application.DTOs;

namespace Application.Abstractions;

public interface IManagerRepository : ICRUDRepository<ManagerDto, int>
{
    Task<IEnumerable<ManagerDto>> GetAllWithUsersAsync();
    Task<ManagerDto> GetByUserIdAsync(int id);
}
