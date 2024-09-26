using Application.Common;

namespace Application.DTOs;

public class ManagerDto : BaseDto<int>
{
    public int UserId { get; set; }
    public string Department { get; set; }
    public DateTime HireDate { get; set; }
    public ICollection<ProjectDto> Projects { get; set; }
}
