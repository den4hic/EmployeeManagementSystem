using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class ManagerDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Department { get; set; }
    public UserDto User { get; set; }
    public ICollection<ProjectDto> Projects { get; set; }
}
