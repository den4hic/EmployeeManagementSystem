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

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects))
            .ReverseMap();

        CreateMap<Manager, ManagerDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.Projects, opt => opt.MapFrom(src => src.Projects))
            .ReverseMap();

        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
            .ForMember(dest => dest.Managers, opt => opt.MapFrom(src => src.Managers))
            .ReverseMap();

        CreateMap<Status, StatusDto>().ReverseMap();

        CreateMap<Task, TaskDto>()
            .ForMember(dest => dest.AssignedToEmployee, opt => opt.MapFrom(src => src.AssignedToEmployee))
            .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ReverseMap();
    }
}