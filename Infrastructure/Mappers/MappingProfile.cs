using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System.Net;
using Task = Domain.Entities.Task;

namespace Infrastructure.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<Employee, EmployeeDto>().ReverseMap();

        CreateMap<Manager, ManagerDto>().ReverseMap();

        CreateMap<Project, ProjectDto>().ReverseMap();

        CreateMap<Status, StatusDto>().ReverseMap();

        CreateMap<Task, TaskDto>().ReverseMap();

        CreateMap<RegisterDto, UserDto>().ReverseMap();

        CreateMap<EmployeeDto, EmployeeManagerRoleDto>().ReverseMap();

        CreateMap<ManagerDto, EmployeeManagerRoleDto>().ReverseMap();
    }
}