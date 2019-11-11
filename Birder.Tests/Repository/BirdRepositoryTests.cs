using Birder.Data;
using Birder.Data.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Birder.Tests.Repository
{
    public class BirdRepositoryTests
    {
        [Fact]
        public async System.Threading.Tasks.Task GetAuthor_EmptyGuid_ThrowsArgumentException()
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
