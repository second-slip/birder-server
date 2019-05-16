using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Birder.Data.Model
{
    public class ApplicationUser : IdentityUser
    {
        public double DefaultLocationLatitude { get; set; }

        public double DefaultLocationLongitude { get; set; }

        public string Avatar { get; set; }

        public DateTime RegistrationDate { get; set; }

        public ICollection<Observation> Observations { get; set; }

        //public ICollection<Tag> Tags { get; set; }

        public ICollection<Network> Following { get; set; }

        public ICollection<Network> Followers { get; set; }
    }
}

   

