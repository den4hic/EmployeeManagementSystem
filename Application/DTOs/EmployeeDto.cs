using Domain.Entities;

namespace Application.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Position { get; set; }
    public DateOnly HireDate { get; set; }
    public decimal Salary { get; set; }
    public UserDto User { get; set; }
    public ICollection<TaskDto> Tasks { get; set; }
    public ICollection<ProjectDto> Projects { get; set; }
}
