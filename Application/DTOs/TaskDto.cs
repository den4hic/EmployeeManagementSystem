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
    public int ProjectId { get; set; }
    public int? AssignedToEmployeeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StatusId { get; set; }
    public DateOnly? DueDate { get; set; }
    public EmployeeDto AssignedToEmployee { get; set; }
    public ProjectDto Project { get; set; }
    public StatusDto Status { get; set; }
}
