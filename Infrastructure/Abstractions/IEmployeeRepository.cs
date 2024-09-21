﻿using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions;

public interface IEmployeeRepository : ICRUDRepository<EmployeeDto, int>
{
}