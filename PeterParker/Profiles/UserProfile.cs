using AutoMapper;
using PeterParker.Data.DTOs;
using PeterParker.Data.Models;
using PeterParker.DTOs;

namespace PeterParker.Profiles;

public class UserProfile : Profile
{
    private readonly DataContext context;

    public UserProfile(DataContext context)
    {
        this.context = context;
    }
    public UserProfile()
    {
        CreateMap<User, UserDataDTO>();

        CreateMap<UserRegisterDTO, User>()
            .ForMember(dest =>
                dest.UserName,
                opt => opt.MapFrom(src => src.Email));

        CreateMap<UserLoginDTO, User>()
            .ForMember(dest =>
                dest.UserName,
                opt => opt.MapFrom(src => src.Email));

        CreateMap<UserLoginDTO, User>();

        CreateMap<VehicleDTO, Vehicle>()
            .ForMember(dest =>
                dest.User,
                opt => opt.Ignore() // <-- Doesn't work
                );

        CreateMap<Vehicle, VehicleDTO>()
            .ForMember(dest =>
                dest.UserEmail,
                opt => opt.MapFrom(src => src.User.Email)
                );

        CreateMap<TicketDTO, Ticket>();

        CreateMap<Ticket, TicketDTO>();

        CreateMap<Subscription, SubscriptionDTO>();

        CreateMap<SubscriptionDTO, Subscription>();

        CreateMap<ZoneDTO, Zone>();

        CreateMap<Zone, ZoneDataDTO>()
            .ForMember(dest =>
            dest.ParkingAreas,
            opt => opt.MapFrom(src => src.ParkingAreas)
            );

        CreateMap<Zone, ZoneDTO>();

        CreateMap<ParkingSpace, ParkingSpaceDTO>()
            .ForMember(dest =>
            dest.Vehicle,
            opt => opt.MapFrom(src => src.Vehicle)
            );

        CreateMap<ParkingAreaDTO, ParkingArea>()
            .ForMember(dest =>
            dest.ParkingSpaces,
            opt => opt.Ignore()
            );

        CreateMap<ParkingArea, ParkingAreaDTO>()
            .ForMember(dest =>
            dest.ParkingSpaces,
            opt => opt.MapFrom(src => src.ParkingSpaces)
            );

        CreateMap<Pass, PassDTO>()
            .ForMember(dest =>
            dest.Zones,
            opt => opt.MapFrom(src => src.Zones)
            );
    }
}
