using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class StatusDto : BaseDto<int>
{
    public string Name { get; set; }
}
