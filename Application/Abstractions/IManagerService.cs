using Application.Common;
using Application.DTOs;

namespace Application.Abstractions
{
    public interface IManagerService
    {
        Task<Result<ManagerDto>> CreateManagerAsync(ManagerDto managerDto);
        Task<Result<ManagerDto>> GetManagerByIdAsync(int id);
        Task<Result<IEnumerable<ManagerDto>>> GetAllManagersAsync();
        Task<Result<bool>> UpdateManagerAsync(ManagerDto managerDto);
        Task<Result<bool>> DeleteManagerAsync(int id);
    }
}
