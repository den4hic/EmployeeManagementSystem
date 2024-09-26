using Application.DTOs;

namespace Application.Abstractions;

public interface IStatusService
{
    Task<StatusDto> CreateStatusAsync(StatusDto statusDto);
    Task<StatusDto> GetStatusByIdAsync(int id);
    Task<IEnumerable<StatusDto>> GetAllStatusesAsync();
    Task UpdateStatusAsync(StatusDto statusDto);
    Task DeleteStatusAsync(int id);
}
