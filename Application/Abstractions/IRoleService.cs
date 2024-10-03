using Application.Common;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions;

public interface IRoleService
{
    Task<Result<bool>> CreateDefaultRolesAsync();
    Task<Result<bool>> CreateRoleAsync(string roleName);
    Task<Result<IEnumerable<IdentityRole>>> GetRolesAsync();
    Task<Result<IdentityRole>> GetRoleByIdAsync(string id);
    Task<Result<bool>> DeleteRoleAsync(string roleName);
    Task<Result<bool>> AssignRoleAsync(int userId, string role, EmployeeManagerRoleDto roleData);
}
