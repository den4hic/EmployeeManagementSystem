using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface IEmployeeService
{
    Task<Result<EmployeeDto>> CreateEmployeeAsync(EmployeeDto employeeDto);
    Task<Result<EmployeeDto>> GetEmployeeByIdAsync(int id);
    Task<Result<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync();
    Task<Result<bool>> UpdateEmployeeAsync(EmployeeDto employeeDto);
    Task<Result<bool>> DeleteEmployeeAsync(int id);
}