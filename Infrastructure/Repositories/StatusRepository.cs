﻿using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Infrastructure.Repositories;

public class StatusRepository : CRUDRepositoryBase<Status, StatusDto, EmployeeManagementSystemDbContext, int>, IStatusRepository
{
    public StatusRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
