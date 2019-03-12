using AutoMapper;
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
              .ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName))
              .ReverseMap();
        }
    }
}
