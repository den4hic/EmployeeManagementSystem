using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class ManagerRepository : CRUDRepositoryBase<Manager, ManagerDto, EmployeeManagementSystemDbContext, int>, IManagerRepository
{
    public ManagerRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
