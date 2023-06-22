using TestSupport.EfHelpers;

namespace Birder.Tests.Data_Persistence;

public class ObsRepoTests
{
    [Fact]
    public async Task GetObservationAsync_Should_Return_Observation_Without_Related_Data()
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
                Latitude = 1,
                Longitude = 2,
                FormattedAddress = "",
                ShortAddress = ""
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
                Latitude = 1,
                Longitude = 2,
                FormattedAddress = "",
                ShortAddress = ""
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var n = new ObservationNote()
        {
            Note = "",
            NoteType = ObservationNoteType.General,
            Observation = context.Observations.FirstOrDefault()
        };

        context.ObservationNotes.Add(n);
        context.SaveChanges();
        context.ObservationNotes.Count().ShouldEqual(1);

        context.ChangeTracker.Clear(); //without this related entities are loaded automatically

        var service = new ObservationRepository(context);

        // Act
        var actual = await service.GetObservationAsync(1, false);

        // Assert
        Assert.IsType<Observation>(actual);
        actual.ObservationId.ShouldEqual(1);

        Assert.Null(actual.Bird);
        Assert.Null(actual.Position);
        Assert.Null(actual.Notes);
        Assert.Null(actual.Bird);
        Assert.Null(actual.ObservationTags);

        Assert.NotNull(actual.ApplicationUser);
    }

    [Fact]
    public async Task GetObservationAsync_Observation_With_Related_Data()
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
                Latitude = 1,
                Longitude = 2,
                FormattedAddress = "",
                ShortAddress = ""
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
                Latitude = 1,
                Longitude = 2,
                FormattedAddress = "",
                ShortAddress = ""
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        var n = new ObservationNote()
        {
            Note = "",
            NoteType = ObservationNoteType.General,
            Observation = context.Observations.FirstOrDefault()
        };
        context.ObservationNotes.Add(n);
        context.SaveChanges();
        context.ObservationNotes.Count().ShouldEqual(1);

        context.ChangeTracker.Clear(); //without this related entities are loaded automatically

        var service = new ObservationRepository(context);

        // Act
        var actual = await service.GetObservationAsync(1, true);

        // Assert
        Assert.IsType<Observation>(actual);
        actual.ObservationId.ShouldEqual(1);

        Assert.NotNull(actual.Bird);
        Assert.NotNull(actual.Position);
        Assert.NotNull(actual.Notes);
        Assert.NotNull(actual.Bird);
        Assert.NotNull(actual.ObservationTags);
        Assert.NotNull(actual.ApplicationUser);
    }

    [Fact]
    public async Task GetObservationAsync_If_No_Record_Should_Return_Default_Null()
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
                Latitude = 1,
                Longitude = 2,
                FormattedAddress = "",
                ShortAddress = ""
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
                Latitude = 1,
                Longitude = 2,
                FormattedAddress = "",
                ShortAddress = ""
            }
        };

        context.Observations.Add(ob2);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(2);

        context.ChangeTracker.Clear(); //without this related entities are loaded automatically

        var service = new ObservationRepository(context);

        // Act
        var actual = await service.GetObservationAsync(3, false);

        // Assert
        Assert.Null(actual);
    }
}