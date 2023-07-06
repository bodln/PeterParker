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
        CreateMap<UserDTO, User>()
            .ForMember(dest =>
                dest.UserName,
                opt => opt.MapFrom(src => src.Email));
        CreateMap<User, UserDTO>();
        CreateMap<VehicleDTO, Vehicle>()
            .ForMember(dest =>
                dest.User,
                opt => opt.MapFrom(src => context!.Users.First(u => u.Email == src.UserEmail)) // <-- Doesn't work
                );
        CreateMap<Vehicle, VehicleDTO>()
            .ForMember(dest =>
                dest.UserEmail,
                opt => opt.MapFrom(src => src.User.Email)
                );
    }
}
