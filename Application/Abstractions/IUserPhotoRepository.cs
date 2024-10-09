using Application.DTOs;

namespace Application.Abstractions;

public interface IUserPhotoRepository : ICRUDRepository<UserPhotoDto, int>
{
}
