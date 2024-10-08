﻿using Application.Common;

namespace Application.DTOs;

public class ProjectCreateDto : BaseDto<int>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int StatusId { get; set; }

    public List<int> TaskIds { get; set; } = new List<int>();
    public List<int> ManagerIds { get; set; } = new List<int>();
    public List<int> EmployeeIds { get; set; } = new List<int>();

}
