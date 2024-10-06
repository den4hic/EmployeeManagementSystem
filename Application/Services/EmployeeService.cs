using Application.Abstractions;
using Application.Common;
using Application.DTOs;

namespace Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<EmployeeDto>> CreateEmployeeAsync(EmployeeDto employeeDto)
    {
        try
        {
            var employee = await _employeeRepository.CreateAsync(employeeDto);
            return Result<EmployeeDto>.Success(employee);
        }
        catch (Exception ex)
        {
            return Result<EmployeeDto>.Failure(ex.Message);
        }
    }

    public async Task<Result<EmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee != null
            ? Result<EmployeeDto>.Success(employee)
            : Result<EmployeeDto>.Failure($"Employee with id {id} not found");
    }

    public async Task<Result<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync()
    {
        try
        {
            var employees = await _employeeRepository.GetAllWithUsersAsync();
            return Result<IEnumerable<EmployeeDto>>.Success(employees);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<EmployeeDto>>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateEmployeeAsync(EmployeeDto employeeDto)
    {
        try
        {
            await _employeeRepository.UpdateAsync(employeeDto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteEmployeeAsync(int id)
    {
        try
        {
            await _employeeRepository.DeleteAsync(id);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }
}
