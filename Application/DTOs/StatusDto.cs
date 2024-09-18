using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class StatusDto
    {
        public string? Name { get; set; }

        public ICollection<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}
