using Application.DTOs;

namespace Application.Abstractions;

public interface IUserRepository : ICRUDRepository<UserDto, int>
{
    Task<UserDto> GetByAspNetUserId(string id);
}
