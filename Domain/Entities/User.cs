﻿using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public partial class User : IEntity<int>
{
    public int Id { get; set; }

    public string AspNetUserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual IdentityUser AspNetUser { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual Manager? Manager { get; set; }
}
