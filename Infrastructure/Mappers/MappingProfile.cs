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

        CreateMap<NotificationGroup, NotificationGroupDto>().ReverseMap();

        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.Group, opt => opt.MapFrom(src => new NotificationGroupDto
            {
                Id = src.Group.Id,
                Name = src.Group.Name
            }))
            .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => new UserDto
            {
                Id = src.Receiver.Id,
                FirstName = src.Receiver.FirstName,
                LastName = src.Receiver.LastName,
            }))
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => new UserDto
            {
                Id = src.Sender.Id,
                FirstName = src.Sender.FirstName,
                LastName = src.Sender.LastName,
            })).ReverseMap();

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