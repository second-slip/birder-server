using TestSupport.EfHelpers;

namespace Birder.Tests.Data_Persistence;

public class RepositoryTests
{
    [Fact]
    public async Task GetAsync_Should_Return_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var b1 = new Bird()
        {
            BirdId = 1,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b1);

        var b2 = new Bird()
        {
            BirdId = 2,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b2);
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(2);

        var service = new Repository<Bird>(context);

        // Act
        var actual = await service.GetAsync(1);

        // Assert
        Assert.IsType<Bird>(actual);
        actual.BirdId.ShouldEqual(1); //b1.BirdId
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var b1 = new Bird()
        {
            BirdId = 1,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b1);

        var b2 = new Bird()
        {
            BirdId = 2,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b2);

        context.SaveChanges();
        context.Birds.Count().ShouldEqual(2);

        var service = new Repository<Bird>(context);

        // Act
        var actual = await service.GetAllAsync();

        // Assert
        Assert.IsAssignableFrom<IEnumerable<Bird>>(actual);
        Assert.Equal(2, actual.Count());
        actual.FirstOrDefault().BirdId.ShouldEqual(1); //b1.BirdId
    }

    [Fact]
    public async Task FindAsync_Should_Return_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var b1 = new Bird()
        {
            BirdId = 1,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b1);

        var b2 = new Bird()
        {
            BirdId = 2,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b2);

        context.SaveChanges();
        context.Birds.Count().ShouldEqual(2);

        var service = new Repository<Bird>(context);

        // Act
        var actual = await service.FindAsync(id => id.BirderStatus == BirderStatus.Common);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<Bird>>(actual);
        Assert.Equal(2, actual.Count());
        actual.FirstOrDefault().BirdId.ShouldEqual(1); //b1.BirdId
    }

    [Fact]
    public async Task SingleOrDefaultAsync_Should_Return_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var b1 = new Bird()
        {
            BirdId = 1,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b1);

        var b2 = new Bird()
        {
            BirdId = 2,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b2);
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(2);

        var service = new Repository<Bird>(context);

        // Act
        var actual = await service.SingleOrDefaultAsync(id => id.BirdId == 2);

        // Assert
        Assert.IsType<Bird>(actual);
        Assert.Equal(2, actual.BirdId);
    }

    [Fact]
    public void Add_Should_Add_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Birds.Count().ShouldEqual(0);

        var b1 = new Bird()
        {
            BirdId = 1,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };

        var service = new Repository<Bird>(context);

        // Act
        service.Add(b1);

        context.Birds.Count().ShouldEqual(0);

        context.SaveChanges();

        // Assert
        context.Birds.Count().ShouldEqual(1);
        context.Birds.FirstOrDefault().BirdId.ShouldEqual(1); //i.e.: b1.BirdId
    }

    [Fact]
    public void AddRange_Should_Add_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Birds.Count().ShouldEqual(0);

        var b1 = new List<Bird>
        {
            new()
            {
                BirdId = 1,
                BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
                BirderStatus = BirderStatus.Common,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "",
                CreationDate = DateTime.Now,
                LastUpdateDate = DateTime.Now
            },
            new()
            {
                BirdId = 2,
                BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
                BirderStatus = BirderStatus.Common,
                Class = "",
                Order = "",
                Family = "",
                Genus = "",
                Species = "",
                EnglishName = "",
                CreationDate = DateTime.Now,
                LastUpdateDate = DateTime.Now
            }
        };

        var service = new Repository<Bird>(context);

        // Act
        service.AddRange(b1);

        context.Birds.Count().ShouldEqual(0);

        context.SaveChanges();

        // Assert
        context.Birds.Count().ShouldEqual(2);
        context.Birds.FirstOrDefault().BirdId.ShouldEqual(1); //i.e.: b1.BirdId
    }

    [Fact]
    public async Task Remove_Should_Remove_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var b1 = new Bird()
        {
            BirdId = 1,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b1);

        var b2 = new Bird()
        {
            BirdId = 2,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b2);
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(2);

        var service = new Repository<Bird>(context);

        // Act
        var bird = await service.GetAsync(1);

        service.Remove(bird);
        context.SaveChanges();

        // Assert
        context.Birds.Count().ShouldEqual(1);
        context.Birds.ShouldNotContain(bird);
    }

    [Fact]
    public async Task RemoveRange_Should_Remove_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        var b1 = new Bird()
        {
            BirdId = 1,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b1);

        var b2 = new Bird()
        {
            BirdId = 2,
            BirdConservationStatus = context.ConservationStatuses.FirstOrDefault(),
            BirderStatus = BirderStatus.Common,
            Class = "",
            Order = "",
            Family = "",
            Genus = "",
            Species = "",
            EnglishName = "",
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
        context.Birds.Add(b2);
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(2);

        var service = new Repository<Bird>(context);

        // Act
        var birds = await service.GetAllAsync();

        service.RemoveRange(birds);
        context.SaveChanges();

        // Assert
        context.Birds.Count().ShouldEqual(0);
    }
}