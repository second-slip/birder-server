using AutoMapper;
using Birder.Controllers;
using Birder.Data.Model;
using Birder.ViewModels;

namespace Birder.Data
{
    public class BirderMappingProfile : Profile
    {
        public BirderMappingProfile()
        {
            CreateMap<Observation, ObservationViewModel>()
              .ForMember(o => o.ObservationId, ex => ex.MapFrom(o => o.ObservationId))
              .ForMember(a => a.User, b => b.MapFrom(a => a.ApplicationUser))
              .ForMember(d => d.Bird, m => m.MapFrom(d => d.Bird))
              .ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              .ReverseMap();

            CreateMap<ApplicationUser, UserProfileViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              .ForMember(x => x.FollowersCount, y => y.MapFrom(x => x.Followers.Count))
              .ForMember(x => x.FollowingCount, y => y.MapFrom(x => x.Following.Count))
              .ReverseMap();

            CreateMap<Bird, BirdDetailViewModel>()
              .ReverseMap();

            CreateMap<Bird, BirdSummaryViewModel>()
              .ForMember(a => a.ConservationStatus, b => b.MapFrom(a => a.BirdConservationStatus.ConservationList))
              .ForMember(a => a.BirderStatus, b => b.MapFrom(a => a.BirderStatus))
              .ReverseMap();

            CreateMap<TweetDay, TweetDayViewModel>()
              .ForMember(d => d.Bird, m => m.MapFrom(d => d.Bird))
             .ReverseMap();
        }
    }
}
