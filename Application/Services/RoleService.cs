using Application.Abstractions;
using Application.Common;
using Application.DTOs;
using AutoMapper;
using Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IManagerRepository _managerRepository;

    public RoleService(
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager,
        IMapper mapper,
        IUserRepository userRepository,
        IEmployeeRepository employeeRepository,
        IManagerRepository managerRepository)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _mapper = mapper;
        _userRepository = userRepository;
        _employeeRepository = employeeRepository;
        _managerRepository = managerRepository;
    }

    public async Task<Result<bool>> CreateDefaultRolesAsync()
    {
        try
        {
            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to create default roles: {ex.Message}");
        }
    }

    public async Task<Result<bool>> CreateRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            return Result<bool>.Failure($"Role '{roleName}' already exists");
        }

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        return result.Succeeded
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }

    public async Task<Result<IEnumerable<IdentityRole>>> GetRolesAsync()
    {
        try
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Result<IEnumerable<IdentityRole>>.Success(roles);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<IdentityRole>>.Failure($"Failed to retrieve roles: {ex.Message}");
        }
    }

    public async Task<Result<IdentityRole>> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        return role != null
            ? Result<IdentityRole>.Success(role)
            : Result<IdentityRole>.Failure($"Role with id '{id}' not found");
    }

    public async Task<Result<bool>> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return Result<bool>.Failure($"Role '{roleName}' not found");
        }

        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded
            ? Result<bool>.Success(true)
            : Result<bool>.Failure($"Failed to delete role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }

    public async Task<Result<bool>> AssignRoleAsync(int userId, UserRole role, EmployeeManagerRoleDto roleData)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Result<bool>.Failure($"User with id '{userId}' not found");

            var userIdentity = await _userManager.FindByIdAsync(user.AspNetUserId);
            if (userIdentity == null)
                return Result<bool>.Failure($"Identity user with id '{user.AspNetUserId}' not found");

            if (!await _roleManager.RoleExistsAsync(role.ToString()))
                return Result<bool>.Failure($"Role '{role}' does not exist");

            var currentRoles = await _userManager.GetRolesAsync(userIdentity);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(userIdentity, currentRoles);
                if (!removeResult.Succeeded)
                    return Result<bool>.Failure($"Failed to remove existing roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");

                if (currentRoles.Contains("Manager"))
                {
                    var manager = await _managerRepository.GetByUserIdAsync(user.Id);
                    if (manager != null)
                        await _managerRepository.DeleteAsync(manager.Id);
                }

                if (currentRoles.Contains("Employee"))
                {
                    var employee = await _employeeRepository.GetByUserIdAsync(user.Id);
                    if (employee != null)
                        await _employeeRepository.DeleteAsync(employee.Id);
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(userIdentity, role.ToString());
            if (!addRoleResult.Succeeded)
                return Result<bool>.Failure($"Failed to add role: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");

            switch (role)
            {
                case UserRole.Manager:
                    var managerDto = _mapper.Map<ManagerDto>(roleData);
                    managerDto.UserId = user.Id;
                    await _managerRepository.CreateAsync(managerDto);
                    break;
                case UserRole.Employee:
                    var employeeDto = _mapper.Map<EmployeeDto>(roleData);
                    employeeDto.UserId = user.Id;
                    await _employeeRepository.CreateAsync(employeeDto);
                    break;
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to assign role: {ex.Message}");
        }
    }
}