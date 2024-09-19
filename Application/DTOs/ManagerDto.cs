using Application.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class ManagerDto : BaseDto<int>
{
    public int UserId { get; set; }
    public string Department { get; set; }
    public ICollection<ProjectDto> Projects { get; set; }
}
