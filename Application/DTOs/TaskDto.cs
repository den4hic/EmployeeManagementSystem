using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public int? EmployeeId { get; set; }
    public int? ProjectId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int? StatusId { get; set; }
}
