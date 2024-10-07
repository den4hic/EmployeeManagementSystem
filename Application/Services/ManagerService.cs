using Application.Abstractions;
using Application.Common;
using Application.DTOs;

namespace Application.Services
{
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        public async Task<Result<ManagerDto>> CreateManagerAsync(ManagerDto managerDto)
        {
            try
            {
                var manager = await _managerRepository.CreateAsync(managerDto);
                return Result<ManagerDto>.Success(manager);
            }
            catch (Exception ex)
            {
                return Result<ManagerDto>.Failure(ex.Message);
            }
        }

        public async Task<Result<ManagerDto>> GetManagerByIdAsync(int id)
        {
            var manager = await _managerRepository.GetByIdAsync(id);
            return manager != null
                ? Result<ManagerDto>.Success(manager)
                : Result<ManagerDto>.Failure($"Manager with id {id} not found");
        }

        public async Task<Result<IEnumerable<ManagerDto>>> GetAllManagersAsync()
        {
            try
            {
                var managers = await _managerRepository.GetAllWithUsersAsync();
                return Result<IEnumerable<ManagerDto>>.Success(managers);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ManagerDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> UpdateManagerAsync(ManagerDto managerDto)
        {
            try
            {
                await _managerRepository.UpdateAsync(managerDto);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteManagerAsync(int id)
        {
            try
            {
                await _managerRepository.DeleteAsync(id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }
    }

}
