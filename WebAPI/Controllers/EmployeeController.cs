using Application.DTOs;
using AutoMapper;
using Domain.Entities;
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
    private readonly EmployeeManagementSystemDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeController(EmployeeManagementSystemDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var employee = _mapper.Map<Employee>(employeeDto);

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        var createdEmployeeDto = _mapper.Map<EmployeeDto>(employee);

        return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployeeDto.Id }, createdEmployeeDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _context.Employees
            .Include(e => e.Tasks)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            return NotFound();
        }

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return Ok(employeeDto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees()
    {
        var employees = await _context.Employees
            .Include(e => e.Tasks)
            .ToListAsync();

        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return Ok(employeeDtos);
    }
}
