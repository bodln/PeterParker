using AutoMapper;
using PeterParker.Data.Models;
using PeterParker.DTOs;

namespace PeterParker.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDTO, User>()
            .ForMember(dest =>
                dest.UserName,
                opt => opt.MapFrom(src => src.Email));
    }
}
