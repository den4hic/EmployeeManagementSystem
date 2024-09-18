using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Project
{
    public int Id { get; set; }

    public int? ManagerId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
