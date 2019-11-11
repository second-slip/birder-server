using Birder.Data;
using Birder.Data.Model;
using Birder.Data.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Birder.Tests.Repository
{
    public class BirdRepositoryTests
    {
        [Theory]
        [InlineData(5)]
        [InlineData(25)]
        [InlineData(10)]
        public async Task GetBirdsAsync_PageSizeTheory_ReturnsPageSize(int pageSize)
        {
            // Arrange

            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.ConservationStatuses.Add(new ConservationStatus() { ConservationStatusId = 1, ConservationList = "Red", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                context.ConservationStatuses.Add(new ConservationStatus() { ConservationStatusId = 2, ConservationList = "Amber", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });
                context.ConservationStatuses.Add(new ConservationStatus() { ConservationStatusId = 3, ConservationList = "Green", Description = "", CreationDate = DateTime.Now, LastUpdateDate = DateTime.Now });

                for (int i = 1; i < 30; i++)
                {
                    Random r = new Random();
                    context.Birds.Add(new Bird()
                    {
                        BirdId = i,
                        Class = $"Class {i}",
                        Order = $"Order {i}",
                        Family = $"Family {i}",
                        Genus = $"Genus {i}",
                        Species = $"Species {i}",
                        EnglishName = $"Name {i}",
                        ConservationStatusId = r.Next(1, 3),
                        CreationDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now
                    });
                }

                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var birdRepository = new BirdRepository(context);

                // Act
                var birds = await birdRepository.GetBirdsAsync(1, pageSize);

                // Assert
                Assert.Equal(pageSize, birds.Items.Count());
                Assert.IsType<ConservationStatus>(birds.Items.First().BirdConservationStatus);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task GetBird_EmptyId_ThrowsArgumentException()
        {
            // Arrange
            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var authorRepository = new BirdRepository(context);

                // Assert
                await Assert.ThrowsAsync<ArgumentException>(
                    // Act      
                    () => authorRepository.GetBirdAsync(0));
            }
        }
    }
}
