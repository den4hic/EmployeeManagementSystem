using Domain.Entities;

namespace Application.DTOs;

public class EmployeeDto
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? PhoneNumber { get; set; }

    public string? HireDate { get; set; }

    public ManagerDto? Manager { get; set; }

    public ICollection<TaskDto> Tasks { get; set; } = new List<TaskDto>();
}
