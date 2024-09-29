using Application.Common;
using Domain.Entities;

namespace Application.DTOs;

public class EmployeeDto : BaseDto<int>
{
    public int UserId { get; set; }
    public string Position { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public virtual ICollection<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    public virtual ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
}
