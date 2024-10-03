using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface IStatusService
{
    Task<Result<StatusDto>> CreateStatusAsync(StatusDto statusDto);
    Task<Result<StatusDto>> GetStatusByIdAsync(int id);
    Task<Result<IEnumerable<StatusDto>>> GetAllStatusesAsync();
    Task<Result<bool>> UpdateStatusAsync(StatusDto statusDto);
    Task<Result<bool>> DeleteStatusAsync(int id);
}
