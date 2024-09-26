using Application.Common;

namespace Application.DTOs;

public class TaskDto : BaseDto<int>
{
    public int ProjectId { get; set; }
    public int? AssignedToEmployeeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int StatusId { get; set; }
    public DateTime? DueDate { get; set; }
    public EmployeeDto? AssignedToEmployee { get; set; }
    public ProjectDto? Project { get; set; }
    public StatusDto? Status { get; set; }
}
