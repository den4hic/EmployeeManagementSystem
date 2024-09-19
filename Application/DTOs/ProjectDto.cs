using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int StatusId { get; set; }
    public StatusDto Status { get; set; }
    public ICollection<TaskDto> Tasks { get; set; }
    public ICollection<EmployeeDto> Employees { get; set; }
    public ICollection<ManagerDto> Managers { get; set; }
}
