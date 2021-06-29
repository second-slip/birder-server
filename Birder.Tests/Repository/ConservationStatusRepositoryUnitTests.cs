using Birder.Data;
using Birder.Data.Repository;
using System.Linq;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.Repository
{
    public class ConservationStatusRepositoryUnitTests
    {
        [Fact]
        public async Task Repsoitory_Returns_String()
        {
            // Arrange
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                context.CreateEmptyViaDelete();
                context.Database.EnsureCreated();

                // N.b. ConservationStatus are added in ApplicationDbContext.cs
                context.ConservationStatuses.Count().ShouldEqual(5);

                var repository = new ConservationStatusRepository(context);

                // Act
                var actual = await repository.GetFirstConservationListStatusAsync();// .GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == "Does Not Exist");
                
                // Assert
                actual.ShouldBeType<string>();
                actual.ShouldEqual("Red");
            }
        }
    }
}