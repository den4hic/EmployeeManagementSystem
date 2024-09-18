using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class TaskDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public EmployeeDto? Employee { get; set; }

        public ProjectDto? Project { get; set; }

        public StatusDto? Status { get; set; }
    }
}
