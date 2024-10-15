using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Repositories;

public class NotificationRepository : CRUDRepositoryBase<Notification, NotificationDto, EmployeeManagementSystemDbContext, int>, INotificationRepository
{
    public NotificationRepository(EmployeeManagementSystemDbContext context, IMapper mapper) : base(context, mapper)
    {
    }
}
