using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
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

    public async Task<bool> CreateDefaultRolesAsync()
    {
        string[] roleNames = { "User", "Admin", "Employee", "Manager" };

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        return true;
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            return false;
        }

        await _roleManager.CreateAsync(new IdentityRole(roleName));
        return true;
    }

    public async Task<IEnumerable<IdentityRole>> GetRolesAsync()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<IdentityRole> GetRoleByIdAsync(string id)
    {
        return await _roleManager.FindByIdAsync(id);
    }

    public async Task<bool> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
            return true;
        }
        return false;
    }

    public async Task AssignRoleAsync(string username, string role, EmployeeManagerRoleDto roleData)
    {
        var userIdentity = await _userManager.FindByNameAsync(username);
        if (userIdentity == null)
            throw new Exception("User not found");

        if (!await _roleManager.RoleExistsAsync(role))
            throw new Exception("Role does not exist");

        await _userManager.AddToRoleAsync(userIdentity, role);

        var user = await _userRepository.GetByAspNetUserIdAsync(userIdentity.Id);

        if (role == "Manager")
        {
            var managerDto = _mapper.Map<ManagerDto>(roleData);
            managerDto.UserId = user.Id;
            await _managerRepository.CreateAsync(managerDto);
        }
        else if (role == "Employee")
        {
            var employeeDto = _mapper.Map<EmployeeDto>(roleData);
            employeeDto.UserId = user.Id;
            await _employeeRepository.CreateAsync(employeeDto);
        }
    }
}
