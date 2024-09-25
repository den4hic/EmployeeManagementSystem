using Application.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class ProjectDto : BaseDto<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int StatusId { get; set; }
    public ICollection<TaskDto> Tasks { get; set; }
    public ICollection<EmployeeDto> Employees { get; set; }
    public ICollection<ManagerDto> Managers { get; set; }
}
