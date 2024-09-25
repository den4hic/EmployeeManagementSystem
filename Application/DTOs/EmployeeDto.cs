using Application.Common;
using Domain.Entities;

namespace Application.DTOs;

public class EmployeeDto : BaseDto<int>
{
    public int UserId { get; set; }
    public string Position { get; set; }
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public ICollection<TaskDto> Tasks { get; set; }
    public ICollection<ProjectDto> Projects { get; set; }
}
