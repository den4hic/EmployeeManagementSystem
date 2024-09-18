using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public int? ManagerId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? PhoneNumber { get; set; }

    public string? HireDate { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
