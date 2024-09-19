using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Task : IEntity<int>
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int? AssignedToEmployeeId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusId { get; set; }

    public DateOnly? DueDate { get; set; }

    public virtual Employee? AssignedToEmployee { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
