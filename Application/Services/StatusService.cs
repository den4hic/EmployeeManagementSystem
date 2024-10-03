using Application.Abstractions;
using Application.Common;
using Application.DTOs;

namespace Application.Services;

public class StatusService : IStatusService
{
    private readonly IStatusRepository _statusRepository;

    public StatusService(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    public async Task<Result<StatusDto>> CreateStatusAsync(StatusDto statusDto)
    {
        try
        {
            var status = await _statusRepository.CreateAsync(statusDto);
            return Result<StatusDto>.Success(status);
        }
        catch (Exception ex)
        {
            return Result<StatusDto>.Failure($"Failed to create user: {ex.Message}");
        }
    }

    public async Task<Result<StatusDto>> GetStatusByIdAsync(int id)
    {
        var status = await _statusRepository.GetByIdAsync(id);
        return status != null
            ? Result<StatusDto>.Success(status)
            : Result<StatusDto>.Failure($"Status with id {id} not found");
    }

    public async Task<Result<IEnumerable<StatusDto>>> GetAllStatusesAsync()
    {
        try
        {
            var statuses = await _statusRepository.GetAllAsync();
            return Result<IEnumerable<StatusDto>>.Success(statuses);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<StatusDto>>.Failure($"Failed to retrieve statuses: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateStatusAsync(StatusDto statusDto)
    {
        try
        {
            await _statusRepository.UpdateAsync(statusDto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to update status: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteStatusAsync(int id)
    {
        try
        {
            await _statusRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to delete status: {ex.Message}");
        }
    }
}
