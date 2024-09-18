using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class StatusDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<int> TaskIds { get; set; } = new List<int>();
}
