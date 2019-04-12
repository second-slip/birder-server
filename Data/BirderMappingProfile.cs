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
              .ForMember(x => x.ProfileImage, y => y.MapFrom(x => x.ProfileImage))
              .ForMember(x => x.DefaultLocationLatitude, y => y.MapFrom(x => x.DefaultLocationLatitude))
              .ForMember(x => x.DefaultLocationLongitude, y => y.MapFrom(x => x.DefaultLocationLongitude))
              .ReverseMap();

            CreateMap<ApplicationUser, NetworkUserViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              .ReverseMap();

             CreateMap<ApplicationUser, Network>()
             .ReverseMap();

             CreateMap<Network, NetworkUserViewModel>()
             .ForMember(x => x.UserName, y => y.MapFrom(x => x.ApplicationUser.UserName))
             .ForMember(x => x.ProfileImage, y => y.MapFrom(x => x.ApplicationUser.ProfileImage))
             .ReverseMap();

            CreateMap<ApplicationUser, UserProfileViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              .ForMember(x => x.Followers, y => y.MapFrom(x => x.Followers))
              .ForMember(x => x.Following, y => y.MapFrom(x => x.Following))
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
