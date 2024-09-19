using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Employee : IEntity<int>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Position { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public decimal Salary { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
