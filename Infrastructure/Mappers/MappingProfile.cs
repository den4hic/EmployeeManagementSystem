using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Task = Domain.Entities.Task;

namespace Infrastructure.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.NotificationGroups, opt => opt.MapFrom(src => src.UserNotificationGroups.Select(ung => ung.Group)));

        CreateMap<UserDto, User>();

        CreateMap<Employee, EmployeeDto>();

        CreateMap<EmployeeDto, Employee>();

        CreateMap<Manager, ManagerDto>().ReverseMap();

        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Managers, opt => opt.MapFrom(src => src.ProjectManagers.Select(pm => pm.Manager)))
            .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.ProjectEmployees.Select(pe => pe.Employee)));

        CreateMap<Project, ProjectCreateDto>().ReverseMap();

        CreateMap<Status, StatusDto>().ReverseMap();

        CreateMap<Task, TaskDto>().ReverseMap();

        CreateMap<RegisterDto, UserDto>().ReverseMap();

        CreateMap<EmployeeDto, EmployeeManagerRoleDto>().ReverseMap();

        CreateMap<ManagerDto, EmployeeManagerRoleDto>().ReverseMap();

        CreateMap<UserPhoto, UserPhotoDto>().ReverseMap();
    }
}