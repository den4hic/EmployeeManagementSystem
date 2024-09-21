using Application.Common;

namespace Application.Abstractions;

public interface ICRUDRepository<TDto, TId>
       where TDto : BaseDto<TId>
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> GetByIdAsync(TId id);
    Task<TDto> CreateAsync(TDto dto);
    Task UpdateAsync(TDto dto);
    Task DeleteAsync(TId id);
}
