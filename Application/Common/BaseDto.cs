using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;

public abstract class BaseDto<TId>
{
    public TId Id { get; set; }
}
