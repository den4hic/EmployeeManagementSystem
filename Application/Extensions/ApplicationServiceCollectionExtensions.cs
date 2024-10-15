using Application.Abstractions;
using Application.DTOs;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IManagerService, ManagerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserPhotoService, UserPhotoService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
        services.AddScoped<IValidator<EmployeeDto>, EmployeeDtoValidator>();
        services.AddScoped<IValidator<ManagerDto>, ManagerDtoValidator>();
        services.AddScoped<IValidator<ProjectDto>, ProjectDtoValidator>();
        services.AddScoped<IValidator<ProjectCreateDto>, ProjectCreateDtoValidator>();
        services.AddScoped<IValidator<StatusDto>, StatusDtoValidator>();
        services.AddScoped<IValidator<TaskDto>, TaskDtoValidator>();

        return services;
    }
}
