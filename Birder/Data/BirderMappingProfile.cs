using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Birder.Data.Model;
using Birder.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Birder.Data
{
    public class BirderMappingProfile : Profile
    {
        public BirderMappingProfile()
        {
            CreateMap<Observation, ObservationDto>()
              .ForMember(a => a.Position, b => b.MapFrom(a => a.Position))
              .ForMember(a => a.User, b => b.MapFrom(a => a.ApplicationUser));

            CreateMap<ObservationDto, Observation>()
              .ForMember(a => a.Position, b => b.MapFrom(a => a.Position))
              .ForMember(a => a.ApplicationUser, b => b.Ignore())
              .ForMember(a => a.CreationDate, b => b.Ignore());

            CreateMap<ObservationEditDto, Observation>()
              .ForMember(a => a.Position, b => b.Ignore())
              .ForMember(a => a.ApplicationUser, b => b.Ignore())
              .ForMember(a => a.CreationDate, b => b.Ignore())
              .ForMember(a => a.BirdId, b => b.Ignore())
              .ForMember(a => a.Bird, b => b.Ignore())
              .ForMember(a => a.Notes, b => b.Ignore())
              .ReverseMap();

            CreateMap<List<Observation>, ObservationAnalysisViewModel>()
              .ForMember(a => a.TotalObservationsCount, b => b.MapFrom(a => a.Count))
              .ForMember(a => a.UniqueSpeciesCount, b => b.MapFrom(a => a.Select(i => i.BirdId).Distinct().Count()));

            CreateMap<QueryResult<Observation>, ObservationFeedDto>()
                .ForMember(a => a.Items, b => b.MapFrom(a => a.Items));

            CreateMap<List<Observation>, List<LifeListViewModel>>();

            CreateMap<ApplicationUser, UserViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              .ForMember(x => x.Avatar, y => y.MapFrom(x => x.Avatar))
              .ForMember(x => x.DefaultLocationLatitude, y => y.MapFrom(x => x.DefaultLocationLatitude))
              .ForMember(x => x.DefaultLocationLongitude, y => y.MapFrom(x => x.DefaultLocationLongitude))
              .ReverseMap();

            CreateMap<ApplicationUser, NetworkUserViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              .ReverseMap();

            CreateMap<ApplicationUser, Network>()
              .ReverseMap();

            CreateMap<Network, FollowingViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.ApplicationUser.UserName))
              .ForMember(x => x.Avatar, y => y.MapFrom(x => x.ApplicationUser.Avatar))
              .ReverseMap();

            CreateMap<Network, FollowerViewModel>()
              .ForMember(x => x.UserName, y => y.MapFrom(x => x.Follower.UserName))
              .ForMember(x => x.Avatar, y => y.MapFrom(x => x.Follower.Avatar))
              .ReverseMap();

            CreateMap<ApplicationUser, UserProfileViewModel>()
              //.ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              //.ForMember(x => x.Followers, y => y.MapFrom(x => x.Followers))
              //.ForMember(x => x.Following, y => y.MapFrom(x => x.Following))
              .ReverseMap();

            CreateMap<ApplicationUser, UserNetworkDto>()
              .ForMember(x => x.Followers, y => y.MapFrom(x => x.Followers))
              .ForMember(x => x.Following, y => y.MapFrom(x => x.Following))
              .ReverseMap();

            CreateMap<ApplicationUser, ManageProfileViewModel>()
              .ReverseMap();

            CreateMap<Bird, BirdDetailDto>()
              .ReverseMap();

            CreateMap<Bird, BirdSummaryViewModel>()
              .ForMember(a => a.ConservationStatus, b => b.MapFrom(a => a.BirdConservationStatus.ConservationList))
              .ForMember(a => a.ConservationListColourCode, b => b.MapFrom(a => a.BirdConservationStatus.ConservationListColourCode))
              .ForMember(a => a.BirderStatus, b => b.MapFrom(a => a.BirderStatus))
              .ReverseMap();

            CreateMap<QueryResult<Bird>, BirdsListDto>()
                .ForMember(a => a.Items, b => b.MapFrom(a => a.Items));

            CreateMap<TweetDay, TweetDayViewModel>()
              .ForMember(d => d.Bird, m => m.MapFrom(d => d.Bird))
              .ReverseMap();

            CreateMap<QueryResult<TweetDay>, TweetArchiveDto>()
                .ForMember(a => a.Items, b => b.MapFrom(a => a.Items));

            CreateMap<ObservationPosition, ObservationPositionDto>()
                .ReverseMap();

            CreateMap<ObservationNoteDto, ObservationNote>()
                .EqualityComparison((odto, o) => odto.Id == o.Id)
                .ForMember(n => n.NoteType, b => b.MapFrom(i => i.NoteType))
                .ForMember(n => n.Note, b => b.MapFrom(i => i.Note))
                .ForMember(r => r.Observation, i => i.UseDestinationValue());

            CreateMap<ObservationNote, ObservationNoteDto>();
        }
    }
}
