using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
