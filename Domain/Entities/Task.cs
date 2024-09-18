using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Task
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProjectId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int? StatusId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Project? Project { get; set; }

    public virtual Status? Status { get; set; }
}
