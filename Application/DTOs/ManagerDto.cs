using Application.Common;
using Domain.Entities;

namespace Application.DTOs;

public class ManagerDto : BaseDto<int>
{
    public int UserId { get; set; }
    public string Department { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public virtual ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
    public UserDto User { get; set; } = null!;
}
