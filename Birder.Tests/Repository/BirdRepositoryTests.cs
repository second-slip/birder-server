using Birder.Data.Repository;
using TestSupport.EfHelpers;

namespace Birder.Tests.Repository;
public class BirdRepositoryTests
{
    [Theory]
    [InlineData(5)]
    [InlineData(25)]
    [InlineData(10)]
    public async Task GetBirdsAsync_PageSizeTheory_ReturnsPageSize(int pageSize)
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

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

        // Act
        var birdRepository = new BirdRepository(context);
        var birds = await birdRepository.GetBirdsAsync(1, pageSize, BirderStatus.Common);

        // Assert
        Assert.Equal(pageSize, birds.Items.Count());
        Assert.IsType<ConservationStatus>(birds.Items.First().BirdConservationStatus);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(25)]
    [InlineData(10)]
    public async Task GetBirdsAsync_PageIndexIsZero_ReturnsPageSize(int pageSize)
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

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

        // Act
        var birdRepository = new BirdRepository(context);
        var birds = await birdRepository.GetBirdsAsync(0, pageSize, BirderStatus.Common);

        // Assert
        Assert.Equal(pageSize, birds.Items.Count());
        Assert.IsType<ConservationStatus>(birds.Items.First().BirdConservationStatus);
    }

    [Fact]
    public async Task GetBirdsAsync_PageSizeIsZero_ReturnsDefaultPageSize()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

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

        // Act
        var birdRepository = new BirdRepository(context);
        var birds = await birdRepository.GetBirdsAsync(1, 0, BirderStatus.Common);

        // Assert
        Assert.Equal(10, birds.Items.Count());
        Assert.IsType<ConservationStatus>(birds.Items.First().BirdConservationStatus);

    }

    [Fact]
    public async Task GetBird_EmptyId_ThrowsArgumentException()
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var authorRepository = new BirdRepository(context);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(
            // Act      
            () => authorRepository.GetBirdAsync(0));
    }
}