using Application.DTOs;

namespace Application.Abstractions;

public interface IEmployeeRepository : ICRUDRepository<EmployeeDto, int>
{
    Task<IEnumerable<EmployeeDto>> GetAllWithUsersAsync();
    Task<EmployeeDto> GetByUserIdAsync(int id);
}
