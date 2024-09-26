using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions;

public interface IRoleService
{
    Task<bool> CreateDefaultRolesAsync();
    Task<bool> CreateRoleAsync(string roleName);
    Task<IEnumerable<IdentityRole>> GetRolesAsync();
    Task<IdentityRole> GetRoleByIdAsync(string id);
    Task<bool> DeleteRoleAsync(string roleName);
    Task AssignRoleAsync(string username, string role, EmployeeManagerRoleDto roleData);
}
