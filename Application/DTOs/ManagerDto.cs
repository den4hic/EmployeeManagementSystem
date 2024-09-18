using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ManagerDto
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? HireDate { get; set; }

        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();

        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    }
}
