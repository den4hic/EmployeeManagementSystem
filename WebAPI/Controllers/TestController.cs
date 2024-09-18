using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly EmployeeManagementSystemDbContext _context;
        private readonly IMapper _mapper;

        public TestController(EmployeeManagementSystemDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Manager> Get()
        {
            var managers = _context.Managers.ToList();

            return managers;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ManagerDto>> GetManagerById(int id)
        {
            var manager = await _context.Managers
                                        .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
            {
                return NotFound();
            }

            var managerDto = _mapper.Map<ManagerDto>(manager);

            return Ok(managerDto);
        }
    }
}
