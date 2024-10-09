using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class UserPhotoRepository : CRUDRepositoryBase<UserPhoto, UserPhotoDto, EmployeeManagementSystemDbContext, int>, IUserPhotoRepository
{
    public UserPhotoRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
