using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ICRUDRepository<EmployeeDto, int> _employeeRepository;

    public EmployeeController(ICRUDRepository<EmployeeDto, int> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdEmployee = await _employeeRepository.CreateAsync(employeeDto);


        return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetEmployee(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return Ok(employees);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateEmployee(EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _employeeRepository.UpdateAsync(employeeDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        await _employeeRepository.DeleteAsync(id);

        return NoContent();
    }
}
