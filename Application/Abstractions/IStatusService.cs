using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface IStatusService
{
    Task<StatusDto> CreateStatusAsync(StatusDto statusDto);
    Task<StatusDto> GetStatusByIdAsync(int id);
    Task<IEnumerable<StatusDto>> GetAllStatusesAsync();
    Task UpdateStatusAsync(StatusDto statusDto);
    Task DeleteStatusAsync(int id);
}
