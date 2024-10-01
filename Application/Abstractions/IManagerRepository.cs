using Application.DTOs;

namespace Application.Abstractions;

public interface IManagerRepository : ICRUDRepository<ManagerDto, int>
{
    Task<ManagerDto> GetByUserIdAsync(int id);
}
