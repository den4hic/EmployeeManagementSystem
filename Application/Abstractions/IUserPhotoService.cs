using Application.Common;
using Application.DTOs;

namespace Application.Abstractions
{
    public interface IUserPhotoService
    {
        Task<Result<UserPhotoDto>> CreateAsync(UserPhotoDto userPhoto);
        Task<Result<UserPhotoDto>> GetUserPhotoById(int id);
        Task<Result<IEnumerable<UserPhotoDto>>> GetAllUserPhotosAsync();
        Task<Result<UserPhotoDto>> GetUserPhotoByUserIdAsync(int userId);
        Task<Result<bool>> UpdateUserPhotoAsync(UserPhotoDto userPhoto);
        Task<Result<bool>> DeleteUserPhotoAsync(int id);
    }
}
