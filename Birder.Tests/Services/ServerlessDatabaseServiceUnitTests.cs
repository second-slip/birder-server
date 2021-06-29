using Birder.Data;
using Birder.Services;
using System.Linq;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.Services
{
    public class ServerlessDatabaseServiceUnitTests
    {
        [Fact]
        public async Task Service_Returns_String()
        {
            //SETUP
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            context.ChangeTracker.Clear(); //NEW LINE ADDED

            // N.b. ConservationStatus are added in ApplicationDbContext.cs
            context.ConservationStatuses.Count().ShouldEqual(5);

            var repository = new ServerlessDatabaseService(context);

            // Act
            var actual = await repository.GetFirstConservationListStatusAsync();// .GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == "Does Not Exist");

            // Assert
            actual.ShouldBeType<string>();
            actual.ShouldEqual("Red");
        }

        // Creates a real db type as production
        //[Fact]
        //public async Task Repsoitory_Returns_String()
        //{
        //    // Arrange
        //    var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        context.CreateEmptyViaDelete();
        //        context.Database.EnsureCreated();

        //        // N.b. ConservationStatus are added in ApplicationDbContext.cs
        //        context.ConservationStatuses.Count().ShouldEqual(5);

        //        var repository = new ConservationStatusRepository(context);

        //        // Act
        //        var actual = await repository.GetFirstConservationListStatusAsync();// .GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == "Does Not Exist");
                
        //        // Assert
        //        actual.ShouldBeType<string>();
        //        actual.ShouldEqual("Red");
        //    }
        //}
    }
}