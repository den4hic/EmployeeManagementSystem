using Application.Abstractions;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _employeeService.CreateEmployeeAsync(employeeDto);

        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetEmployee), new { id = result.Value.Id }, result.Value);
        }

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee.IsSuccess)
        {
            return Ok(employee.Value);
        }

        return NotFound(employee.Error);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();

        if (employees.IsSuccess)
        {
            return Ok(employees.Value);
        }

        return BadRequest(employees.Error);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateEmployee(EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _employeeService.UpdateEmployeeAsync(employeeDto);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}