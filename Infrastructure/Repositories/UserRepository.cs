using Application.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Infrastructure.Repositories;

public class UserRepository : CRUDRepositoryBase<User, UserDto, EmployeeManagementSystemDbContext, int>, IUserRepository
{
    public UserRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<UserDto> GetByAspNetUserId(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.AspNetUserId == id);

        return user != null ? _mapper.Map<UserDto>(user) : null;
    }
}
