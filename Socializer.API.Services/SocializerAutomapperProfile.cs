using AutoMapper;
using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.API.Services;

public class SocializerAutomapperProfile : Profile
{
    public SocializerAutomapperProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<CreateUserDto, User>().ReverseMap();
    }
}
