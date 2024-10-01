using Application.DTOs;

namespace Application.Abstractions;

public interface IEmployeeRepository : ICRUDRepository<EmployeeDto, int>
{
    Task<EmployeeDto> GetByUserIdAsync(int id);
}
