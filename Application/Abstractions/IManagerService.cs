using Application.DTOs;

namespace Application.Abstractions
{
    public interface IManagerService
    {
        Task<ManagerDto> CreateManagerAsync(ManagerDto managerDto);
        Task<ManagerDto> GetManagerByIdAsync(int id);
        Task<IEnumerable<ManagerDto>> GetAllManagersAsync();
        Task UpdateManagerAsync(ManagerDto managerDto);
        Task DeleteManagerAsync(int id);
    }
}
