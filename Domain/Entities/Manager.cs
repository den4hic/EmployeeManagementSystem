using Domain.Common;

namespace Domain.Entities;

public partial class Manager : IEntity<int>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Department { get; set; } = null!;
    public DateTime HireDate { get; set; }

    public virtual User User { get; set; } = null!;


    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
