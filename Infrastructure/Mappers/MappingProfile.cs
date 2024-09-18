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
        CreateMap<Manager, ManagerDto>()
                .ForMember(dest => dest.EmployeeIds, opt => opt.MapFrom(src => src.Employees.Select(e => e.Id)))
                .ForMember(dest => dest.ProjectIds, opt => opt.MapFrom(src => src.Projects.Select(p => p.Id)));

        CreateMap<ManagerDto, Manager>()
            .ForMember(dest => dest.Employees, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore());

        CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.TaskIds, opt => opt.MapFrom(src => src.Tasks.Select(t => t.Id)));

        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.Manager, opt => opt.Ignore());

        CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.TaskIds, opt => opt.MapFrom(src => src.Tasks.Select(t => t.Id)));

        CreateMap<ProjectDto, Project>()
            .ForMember(dest => dest.Tasks, opt => opt.Ignore())
            .ForMember(dest => dest.Manager, opt => opt.Ignore());

        CreateMap<Status, StatusDto>()
                .ForMember(dest => dest.TaskIds, opt => opt.MapFrom(src => src.Tasks.Select(t => t.Id)));

        CreateMap<StatusDto, Status>()
            .ForMember(dest => dest.Tasks, opt => opt.Ignore());

        CreateMap<Task, TaskDto>();

        CreateMap<TaskDto, Task>()
            .ForMember(dest => dest.Employee, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());
    }
}
