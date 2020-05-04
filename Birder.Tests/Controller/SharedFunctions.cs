using Birder.Data;
using Birder.Data.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Birder.Tests.Controller
{
    public static class SharedFunctions
    {
        public static Mock<UserManager<ApplicationUser>> InitialiseMockUserManager()
        {
            return new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        }

        public static UserManager<ApplicationUser> InitialiseUserManager()
        {
            var connectionstring = "Server=(localdb)\\mssqllocaldb;Database=Birder;Trusted_Connection=True;MultipleActiveResultSets=true";

            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionsBuilder.UseSqlServer(connectionstring);

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionstring);


            ApplicationDbContext dbContext = new ApplicationDbContext(optionsBuilder.Options);

            UserStore<ApplicationUser> _userStore = new UserStore<ApplicationUser>(dbContext, null);
            return new UserManager<ApplicationUser>(
                    _userStore,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);
        }

        public static Mock<SignInManager<ApplicationUser>> InitialiseMockSignInManager(Mock<UserManager<ApplicationUser>> userManager)
        {
            return new Mock<SignInManager<ApplicationUser>>(
                        userManager.Object,
                        new Mock<IHttpContextAccessor>().Object,
                        new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                        new Mock<IOptions<IdentityOptions>>().Object,
                        new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                        new Mock<IAuthenticationSchemeProvider>().Object,
                        new Mock<IUserConfirmation<ApplicationUser>>().Object);
        }

        public static ClaimsPrincipal GetTestClaimsPrincipal(string username)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            return user;
        }

        public static IEnumerable<Observation> GetTestObservations(int length, Bird bird)
        {
            var observations = new List<Observation>();
            for (int i = 0; i < length; i++)
            {
                observations.Add(new Observation
                {
                    ObservationId = i,
                    LocationLatitude = 0,
                    LocationLongitude = 0,
                    Quantity = 1,
                    NoteGeneral = "",
                    NoteHabitat = "",
                    NoteWeather = "",
                    NoteAppearance = "",
                    NoteBehaviour = "",
                    NoteVocalisation = "",
                    HasPhotos = false,
                    SelectedPrivacyLevel = PrivacyLevel.Public,
                    ObservationDateTime = DateTime.Now.AddDays(-4),
                    CreationDate = DateTime.Now.AddDays(-4),
                    LastUpdateDate = DateTime.Now.AddDays(-4),
                    ApplicationUserId = "",
                    BirdId = bird.BirdId,
                    Bird = bird,
                    ApplicationUser = null,
                    ObservationTags = null
                });
            }
            return observations;
        }

        public static ApplicationUser GetUser(string username)
        {
            return new ApplicationUser()
            {
                UserName = username
            };
        }
    }
}
