using Application.Extensions;
using FluentValidation.AspNetCore;
using Infrastructure.Context;
using Infrastructure.Extensions;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? throw new InvalidOperationException("Connection string 'AZURE_SQL_CONNECTIONSTRING' not found.");

builder.Services.AddDbContext<EmployeeManagementSystemDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                }).AddEntityFrameworkStores<EmployeeManagementSystemDbContext>()
                    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSecret = builder.Configuration["Jwt:Secret"];
    if (string.IsNullOrEmpty(jwtSecret))
    {
        jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
    }

    if (string.IsNullOrEmpty(jwtSecret))
    {
        throw new InvalidOperationException("JWT secret is not configured");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://yellow-tree-01df2610f.5.azurestaticapps.net")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
        });
});

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddValidators();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.ClientTimeoutInterval = TimeSpan.FromSeconds(15);
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
});

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.ResolveConflictingActions(apiDescription => apiDescription.First());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    });
} else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.Run();
