using Domain.Common;

namespace Domain.Entities;

public partial class Employee : IEntity<int>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Position { get; set; } = null!;

    public DateTime HireDate { get; set; }

    public decimal Salary { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();
}
