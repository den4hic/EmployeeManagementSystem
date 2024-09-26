using Application.DTOs;

namespace Application.Abstractions;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employeeDto);
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task UpdateEmployeeAsync(EmployeeDto employeeDto);
    Task DeleteEmployeeAsync(int id);
}