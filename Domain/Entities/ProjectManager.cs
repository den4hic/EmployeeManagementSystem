namespace Domain.Entities
{
    public class ProjectManager
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int ManagerId { get; set; }
        public Manager Manager { get; set; } = null!;
    }
}
