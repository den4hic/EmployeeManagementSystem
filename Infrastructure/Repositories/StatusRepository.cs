using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class StatusRepository : CRUDRepositoryBase<Status, StatusDto, EmployeeManagementSystemDbContext, int>, IStatusRepository
{
    public StatusRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
