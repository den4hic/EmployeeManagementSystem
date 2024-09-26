using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class ProjectRepository : CRUDRepositoryBase<Project, ProjectDto, EmployeeManagementSystemDbContext, int>, IProjectRepository
{
    public ProjectRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
