using Application.Abstractions;
using Application.DTOs;

namespace Application.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<StatusDto> CreateStatusAsync(StatusDto statusDto)
        {
            return await _statusRepository.CreateAsync(statusDto);
        }

        public async Task<StatusDto> GetStatusByIdAsync(int id)
        {
            return await _statusRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<StatusDto>> GetAllStatusesAsync()
        {
            return await _statusRepository.GetAllAsync();
        }

        public async Task UpdateStatusAsync(StatusDto statusDto)
        {
            await _statusRepository.UpdateAsync(statusDto);
        }

        public async Task DeleteStatusAsync(int id)
        {
            await _statusRepository.DeleteAsync(id);
        }
    }
}
