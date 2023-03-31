using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Birder.TestsHelpers
{
    public static class SharedFunctions
    {

        public static ObservationPosition GetObservationPosition()
        {
            return new ObservationPosition() { };
        }

        public static Observation GetObservation(ApplicationUser user, Bird bird)
        {
            return new Observation
            {
                ApplicationUser = user,
                Bird = bird,
                SelectedPrivacyLevel = PrivacyLevel.Public,
                HasPhotos = true,
                CreationDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                ObservationDateTime = DateTime.Now

            };
        }

        public static Bird GetBird(ConservationStatus status)
        {
            return new Bird
            {
                BirdConservationStatus = status,
                BirderStatus = BirderStatus.Common,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "",
                CreationDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
            };
        }

        public static ClaimsPrincipal GetTestClaimsPrincipal()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            return user;
        }

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

        public static UserManager<ApplicationUser> InitialiseUserManager(ApplicationDbContext context)
        {
            //var connectionstring = "Server=(localdb)\\mssqllocaldb;Database=Birder;Trusted_Connection=True;MultipleActiveResultSets=true";

            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionsBuilder.UseSqlServer(connectionstring);

            //var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //optionsBuilder.UseSqlServer(connectionstring);


            //ApplicationDbContext dbContext = new ApplicationDbContext(optionsBuilder.Options);

            UserStore<ApplicationUser> _userStore = new UserStore<ApplicationUser>(context, null);
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

        public static UserManager<ApplicationUser> InitialiseUserManager()
        {
            var connectionstring = "Server=(localdb)\\mssqllocaldb;Database=Birder;Trusted_Connection=True;MultipleActiveResultSets=true;Connection Timeout=60";

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

        public static NetworkUserViewModel GetTestNetworkUserViewModel(string username)
        {
            return new NetworkUserViewModel() { UserName = username };
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
                    Quantity = 1,
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

        public static ApplicationUser CreateUser(string username)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = string.Concat(username, "@Test.com"),
                NormalizedEmail = string.Concat(username, "@Test.com").ToUpper(),
                PhoneNumber = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = new Guid().ToString("D"),
                Avatar = "",               
            };

            user.PasswordHash = PassGenerate(user);

            return user;
        }

        public static string PassGenerate(ApplicationUser user)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(user, "password");
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
