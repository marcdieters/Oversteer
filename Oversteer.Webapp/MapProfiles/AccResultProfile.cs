using AutoMapper;
using Oversteer.Models.Result;

namespace Oversteer.Webapp.MapProfiles
{
    public class AccResultProfile : Profile
    {
        public AccResultProfile()
        {
            CreateMap<ResultDto, Result>();
            CreateMap<ACCLeaderBoardLine, Leaderboard>();
        }
    }
}
