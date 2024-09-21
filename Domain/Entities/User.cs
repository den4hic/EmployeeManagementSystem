﻿using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class User : IEntity<int>
{
    public int Id { get; set; }

    public string AspNetUserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();
}