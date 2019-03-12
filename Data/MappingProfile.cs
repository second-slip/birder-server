using AutoMapper;
using Birder.Controllers;
using Birder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
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
