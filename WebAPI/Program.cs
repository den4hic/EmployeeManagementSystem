using Application.DTOs;
using Infrastructure.Abstractions;
using Infrastructure.Context;
using Infrastructure.Mappers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ManagementDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ManagementDbContextConnection' not found.");

builder.Services.AddDbContext<EmployeeManagementSystemDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<EmployeeManagementSystemDbContext>();

builder.Services.AddScoped<ICRUDRepository<EmployeeDto, int>, EmployeeRepository>();
builder.Services.AddScoped<ICRUDRepository<StatusDto, int>, StatusRepository>();
builder.Services.AddScoped<ICRUDRepository<UserDto, int>, UserRepository>();
builder.Services.AddScoped<ICRUDRepository<ProjectDto, int>, ProjectRepository>();
builder.Services.AddScoped<ICRUDRepository<ManagerDto, int>, ManagerRepository>();
builder.Services.AddScoped<ICRUDRepository<TaskDto, int>, TaskRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
