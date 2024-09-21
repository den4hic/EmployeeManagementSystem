using Application.DTOs;

namespace Application.Abstractions;

public interface IManagerRepository : ICRUDRepository<ManagerDto, int>
{
}
