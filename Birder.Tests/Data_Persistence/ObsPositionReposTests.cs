using TestSupport.EfHelpers;

namespace Birder.Tests.Data_Persistence;

public class ObsPositionRepoTests
{
    [Fact]
    public async Task GetAsync_Should_Return_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUsername"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob1 = new Observation()
        {
            ObservationId = 1,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 1,
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1"
            }
        };

        context.Observations.Add(ob1);

        var ob2 = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 2,
                Latitude = 2,
                Longitude = 2,
                FormattedAddress = "test 2",
                ShortAddress = "test 2"
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ObservationPositionRepository(context);

        // Act
        var actual = await service.GetAsync(1);

        // Assert
        Assert.IsType<ObservationPosition>(actual);
        actual.ObservationId.ShouldEqual(1); //b1.BirdId
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob1 = new Observation()
        {
            ObservationId = 1,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 1,
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1"
            }
        };

        context.Observations.Add(ob1);

        var ob2 = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 2,
                Latitude = 2,
                Longitude = 2,
                FormattedAddress = "test 2",
                ShortAddress = "test 2"
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ObservationPositionRepository(context);

        // Act
        var actual = await service.GetAllAsync();

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationPosition>>(actual);
        Assert.Equal(2, actual.Count());
        actual.FirstOrDefault().ObservationPositionId.ShouldEqual(1);
    }

    [Fact]
    public async Task FindAsync_Should_Return_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob1 = new Observation()
        {
            ObservationId = 1,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 1,
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1"
            }
        };

        context.Observations.Add(ob1);

        var ob2 = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 2,
                Latitude = 2,
                Longitude = 2,
                FormattedAddress = "test 2",
                ShortAddress = "test 2"
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ObservationPositionRepository(context);

        // Act
        var actual = await service.FindAsync(id => id.ObservationId == 1);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationPosition>>(actual);
        Assert.Single(actual);
        actual.FirstOrDefault().ObservationPositionId.ShouldEqual(1); //b1.BirdId
    }

    [Fact]
    public async Task SingleOrDefaultAsync_Should_Return_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUsername"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob1 = new Observation()
        {
            ObservationId = 1,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 1,
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1"
            }
        };

        context.Observations.Add(ob1);

        var ob2 = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 2,
                Latitude = 2,
                Longitude = 2,
                FormattedAddress = "test 2",
                ShortAddress = "test 2"
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ObservationPositionRepository(context);

        // Act
        var actual = await service.SingleOrDefaultAsync(id => id.ObservationPositionId == 2);

        // Assert
        Assert.IsType<ObservationPosition>(actual);
        Assert.Equal(2, actual.ObservationPositionId);
    }

    [Fact]
    public void Add_Should_Add_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 1,
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1"
            }
        };

        var service = new ObservationPositionRepository(context);

        // Act
        service.Add(ob.Position);
        context.Add(ob);

        context.ObservationPosition.Count().ShouldEqual(0);

        context.SaveChanges();

        // Assert
        context.ObservationPosition.Count().ShouldEqual(1);
        context.ObservationPosition.FirstOrDefault().ObservationPositionId.ShouldEqual(1); //i.e.: b1.BirdId
    }

    [Fact]
    public void AddRange_Should_Add_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUsername"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob1 = new Observation()
        {
            ObservationId = 1,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now
        };

        context.Observations.Add(ob1);

        var ob2 = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var positions = new List<ObservationPosition>
        {
            new()
            {
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1",
                Observation = context.Observations.Where(o => o.ObservationId == 1).FirstOrDefault()
            },
            new ()
            {
                Latitude = 2,
                Longitude = 2,
                FormattedAddress = "test 2",
                ShortAddress = "test 2",
                Observation = context.Observations.Where(o => o.ObservationId == 2).FirstOrDefault()
            }
        };

        var service = new ObservationPositionRepository(context);

        // Act
        service.AddRange(positions);
        context.ObservationPosition.Count().ShouldEqual(0);
        context.SaveChanges();

        // Assert
        context.ObservationPosition.Count().ShouldEqual(2);
        context.ObservationPosition.FirstOrDefault().ObservationPositionId.ShouldEqual(1);
    }

    [Fact]
    public async Task Remove_Should_Remove_Entity()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUsername"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob1 = new Observation()
        {
            ObservationId = 1,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 1,
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1"
            }
        };

        context.Observations.Add(ob1);

        var ob2 = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now,
            Position = new ObservationPosition()
            {
                ObservationPositionId = 2,
                Latitude = 2,
                Longitude = 2,
                FormattedAddress = "test 2",
                ShortAddress = "test 2"
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var service = new ObservationPositionRepository(context);

        // Act
        var position = await service.GetAsync(1);

        service.Remove(position);
        context.SaveChanges();

        // Assert
        context.ObservationPosition.Count().ShouldEqual(1);
        context.ObservationPosition.ShouldNotContain(position);
    }

    [Fact]
    public void RemoveRange_Should_Remove_Entities()
    {
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        context.Users.Add(SharedFunctions.CreateUser("testUsername"));
        context.SaveChanges();
        context.Users.Count().ShouldEqual(1);

        context.Birds.Add(SharedFunctions.GetBird(context.ConservationStatuses.FirstOrDefault()));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(1);

        var ob1 = new Observation()
        {
            ObservationId = 1,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now
        };

        context.Observations.Add(ob1);

        var ob2 = new Observation()
        {
            ObservationId = 2,
            ApplicationUser = context.ApplicationUser.FirstOrDefault(),
            Bird = context.Birds.FirstOrDefault(),
            Quantity = 1,
            SelectedPrivacyLevel = PrivacyLevel.Public,
            HasPhotos = true,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            ObservationDateTime = DateTime.Now
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var positions = new List<ObservationPosition>
        {
            new()
            {
                Latitude = 1,
                Longitude = 1,
                FormattedAddress = "test 1",
                ShortAddress = "test 1",
                Observation = context.Observations.Where(o => o.ObservationId == 1).FirstOrDefault()
            },
            new ()
            {
                Latitude = 2,
                Longitude = 2,
                FormattedAddress = "test 2",
                ShortAddress = "test 2",
                Observation = context.Observations.Where(o => o.ObservationId == 2).FirstOrDefault()
            }
        };

        context.ObservationPosition.AddRange(positions);
        context.SaveChanges();
        context.ObservationPosition.Count().ShouldEqual(2);

        var service = new ObservationPositionRepository(context);

        // Act
        service.RemoveRange(positions);
        context.SaveChanges();

        // Assert
        context.ObservationPosition.Count().ShouldEqual(0);
    }
}