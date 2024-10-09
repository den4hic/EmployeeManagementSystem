using Application.Abstractions;
using Application.Common;
using Application.DTOs;

namespace Application.Services
{
    public class UserPhotoService : IUserPhotoService
    {
        private readonly IUserPhotoRepository _userPhotoRepository;

        public UserPhotoService(IUserPhotoRepository userPhotoRepository)
        {
            _userPhotoRepository = userPhotoRepository;
        }

        public async Task<Result<UserPhotoDto>> CreateAsync(UserPhotoDto userPhoto)
        {
            try
            {
                await _userPhotoRepository.CreateAsync(userPhoto);
                return Result<UserPhotoDto>.Success(userPhoto);
            }
            catch (Exception ex)
            {
                return Result<UserPhotoDto>.Failure($"Failed to create user photo: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteUserPhotoAsync(int id)
        {
            try
            {
                await _userPhotoRepository.DeleteAsync(id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to delete user photo: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<UserPhotoDto>>> GetAllUserPhotosAsync()
        {
            try
            {
                var userPhotos = await _userPhotoRepository.GetAllAsync();
                return Result<IEnumerable<UserPhotoDto>>.Success(userPhotos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UserPhotoDto>>.Failure($"Failed to retrieve user photos: {ex.Message}");
            }
        }

        public async Task<Result<UserPhotoDto>> GetUserPhotoById(int id)
        {
            var userPhoto = await _userPhotoRepository.GetByIdAsync(id);
            return userPhoto != null
                ? Result<UserPhotoDto>.Success(userPhoto)
                : Result<UserPhotoDto>.Failure($"User photo with id {id} not found");
        }

        public Task<Result<UserPhotoDto>> GetUserPhotoByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> UpdateUserPhotoAsync(UserPhotoDto userPhoto)
        {
            try
            {
                await _userPhotoRepository.UpdateAsync(userPhoto);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update user photo: {ex.Message}");
            }
        }
    }
}
