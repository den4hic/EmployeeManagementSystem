using Domain.Entities;

namespace Application.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public int? ManagerId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? HireDate { get; set; }
    public ICollection<int> TaskIds { get; set; } = new List<int>();
}
