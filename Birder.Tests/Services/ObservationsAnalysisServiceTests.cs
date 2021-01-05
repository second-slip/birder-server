using Birder.Data;
using Birder.Data.Model;
using Birder.Services;
using Birder.TestsHelpers;
using Birder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Birder.Tests.Services
{
    public class ObservationsAnalysisServiceTests
    {

        public ObservationsAnalysisServiceTests() { }


        [Fact]
        public async Task GetObservationsSummaryAsync_ReturnsViewModel_WithOneMatchInDb()
        {
            var testUsername = "TestUser1";
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaDelete();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser(testUsername));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(1);

                context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));

                context.SaveChanges();

                context.Birds.Count().ShouldEqual(1);

                context.Observations.Add(SharedFunctions.GetObservation(context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault()));

                context.SaveChanges();

                context.Observations.Count().ShouldEqual(1);


                var service = new ObservationsAnalysisService(context);

                // Act or change
                var actual = await service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == testUsername);

                // Assert
                actual.ShouldBeType<ObservationAnalysisViewModel>();
                actual.TotalObservationsCount.ShouldEqual(1);
                actual.UniqueSpeciesCount.ShouldEqual(1);
            }
        }

        [Fact]
        public async Task GetObservationsSummaryAsync_ReturnsEmptyViewModel_WithNoMatchesInDb()
        {
            var testUsername = "TestUser1";
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaDelete();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser(testUsername));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(1);

                context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));

                context.SaveChanges();

                context.Birds.Count().ShouldEqual(1);

                //context.Observations.Add(SharedFunctions.GetObservation(context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault()));

                //context.SaveChanges();

                context.Observations.Count().ShouldEqual(0);


                var service = new ObservationsAnalysisService(context);

                // Act or change
                var actual = await service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == testUsername);

                // Assert
                actual.ShouldBeType<ObservationAnalysisViewModel>();
                actual.TotalObservationsCount.ShouldEqual(0);
                actual.UniqueSpeciesCount.ShouldEqual(0);
            }
        }

        [Fact]
        public async Task GetObservationsSummaryAsync_ReturnsEmptyViewModel_WhenUserDoesNotExist()
        {
            var testUsername = "TestUser1";
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                //You have to create the database
                context.CreateEmptyViaDelete();
                context.Database.EnsureCreated();

                context.Users.Add(SharedFunctions.CreateUser(testUsername));

                context.SaveChanges();

                context.Users.Count().ShouldEqual(1);

                context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));

                context.SaveChanges();

                context.Birds.Count().ShouldEqual(1);

                //context.Observations.Add(SharedFunctions.GetObservation(context.ApplicationUser.FirstOrDefault(), context.Birds.FirstOrDefault()));

                //context.SaveChanges();

                context.Observations.Count().ShouldEqual(0);


                var service = new ObservationsAnalysisService(context);

                // Act or change
                var actual = await service.GetObservationsSummaryAsync(x => x.ApplicationUser.UserName == "Does Not Exist");

                // Assert
                actual.ShouldBeType<ObservationAnalysisViewModel>();
                actual.TotalObservationsCount.ShouldEqual(0);
                actual.UniqueSpeciesCount.ShouldEqual(0);
            }
        }

        [Fact]
        public async Task GetObservationsSummaryAsync_ReturnsException_WhenArgumentIsNull()
        {
            var options = this.CreateUniqueClassOptions<ApplicationDbContext>();

            using (var context = new ApplicationDbContext(options))
            {
                // Arrange
                context.Database.EnsureClean();
                context.Database.EnsureCreated();

                var service = new ObservationsAnalysisService(context);

                // Act & Assert
                var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.GetObservationsSummaryAsync(null));
                Assert.Equal("The argument is null or empty (Parameter 'predicate')", ex.Message);
            }
        }
    }
}
