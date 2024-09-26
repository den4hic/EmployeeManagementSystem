namespace Application.DTOs;

public class ProjectCreateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int StatusId { get; set; }

    // Список ID існуючих менеджерів, яких потрібно прив'язати до проєкту
    public List<int> ManagerIds { get; set; } = new List<int>();

}
