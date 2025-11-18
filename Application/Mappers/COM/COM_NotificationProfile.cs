using AutoMapper;
using Domain.Entities.MongoDB;
using DTOs.COM.COM_Notification.Requests;
using DTOs.COM.COM_Notification.Responses;

namespace Application.Mappers.COM;

public class COM_NotificationProfile : Profile
{
    public COM_NotificationProfile()
    {
        CreateMap<COM_Notification, NotificationResponse>();
        CreateMap<CreateNotificationRequest, COM_Notification>();
        CreateMap<UpdateNotificationRequest, COM_Notification>()
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
    }
}
