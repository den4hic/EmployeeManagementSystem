using Application.Common;

namespace Application.DTOs;

public class StatusDto : BaseDto<int>
{
    public string Name { get; set; }
}
