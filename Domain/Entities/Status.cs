using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Status
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
