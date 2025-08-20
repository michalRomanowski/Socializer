using AutoMapper;
using Socializer.Database.Models;
using Socializer.Shared.Dtos;

namespace Socializer.Services;

public class SocializerAutomapperProfile : Profile
{
    public SocializerAutomapperProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<CreateUserDto, User>().ReverseMap();

        CreateMap<UserMatch, UserMatchDto>()
            .ConstructUsing(src => new UserMatchDto(
                src.User1.Id,
                src.User2.Id,
                src.User1.Username,
                src.User2.Username,
                src.PreferenceMatches.Select(pm => new PreferenceMatchDto(pm.User1Preference.Preference.DBPediaResource, pm.MatchWeight)),
                src.MatchWeight));

        CreateMap<PreferenceMatch, PreferenceMatchDto>()
            .ConstructUsing(src => new PreferenceMatchDto(
                src.User1Preference.Preference.DBPediaResource,
                src.MatchWeight));
    }
}
