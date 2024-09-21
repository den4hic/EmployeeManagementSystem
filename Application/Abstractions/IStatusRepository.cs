using Application.DTOs;

namespace Application.Abstractions;

public interface IStatusRepository : ICRUDRepository<StatusDto, int>
{
}
