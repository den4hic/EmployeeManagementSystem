using Application.Common;
using AutoMapper;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Application.Abstractions;

public abstract class CRUDRepositoryBase<TEntity, TDto, TContext, TId> : ICRUDRepository<TDto, TId>
            where TEntity : class, IEntity<TId>
            where TDto : BaseDto<TId>
            where TContext : DbContext
{
    protected readonly TContext _context;
    protected readonly IMapper _mapper;
    private readonly Microsoft.EntityFrameworkCore.DbSet<TEntity> _dbSet;

    protected CRUDRepositoryBase(TContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();

        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public async Task<TDto> GetByIdAsync(TId id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity != null ? _mapper.Map<TDto>(entity) : null;
    }

    public async Task<TDto> CreateAsync(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }

    public async Task UpdateAsync(TDto dto)
    {
        var entity = await _dbSet.FindAsync(dto.Id);
        if (entity == null) throw new Exception("Entity not found");

        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TId id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) throw new Exception("Entity not found");
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
