using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class ManagerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? HireDate { get; set; }
    public ICollection<int> EmployeeIds { get; set; } = new List<int>();
    public ICollection<int> ProjectIds { get; set; } = new List<int>();
}
