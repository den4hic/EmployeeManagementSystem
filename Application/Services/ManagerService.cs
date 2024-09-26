using Application.Abstractions;
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

        public async Task<ManagerDto> CreateManagerAsync(ManagerDto managerDto)
        {
            return await _managerRepository.CreateAsync(managerDto);
        }

        public async Task<ManagerDto> GetManagerByIdAsync(int id)
        {
            return await _managerRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ManagerDto>> GetAllManagersAsync()
        {
            return await _managerRepository.GetAllAsync();
        }

        public async Task UpdateManagerAsync(ManagerDto managerDto)
        {
            await _managerRepository.UpdateAsync(managerDto);
        }

        public async Task DeleteManagerAsync(int id)
        {
            await _managerRepository.DeleteAsync(id);
        }
    }

}
