using TestSupport.EfHelpers;

namespace Birder.Tests.Data_Persistence;

public class ObsNotesRepoTests
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
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);

        var service = new ObservationNoteRepository(context);

        // Act
        var actual = await service.GetAsync(1);

        // Assert
        Assert.IsType<ObservationNote>(actual);
        actual.Id.ShouldEqual(1);
        Assert.Equal("test note 1", actual.Note);
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
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);

        var service = new ObservationNoteRepository(context);

        // Act
        var actual = await service.GetAllAsync();

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationNote>>(actual);
        Assert.Equal(2, actual.Count());
        actual.FirstOrDefault().Id.ShouldEqual(1);
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
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);

        var service = new ObservationNoteRepository(context);

        // Act
        var actual = await service.FindAsync(id => id.Id == 1);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ObservationNote>>(actual);
        Assert.Single(actual);
        actual.FirstOrDefault().Id.ShouldEqual(1);
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
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);

        var service = new ObservationNoteRepository(context);

        // Act
        var actual = await service.SingleOrDefaultAsync(id => id.Id == 2);

        // Assert
        Assert.IsType<ObservationNote>(actual);
        Assert.Equal(2, actual.Id);
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
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);

        context.ObservationNotes.Count().ShouldEqual(2);

        var service = new ObservationNoteRepository(context);

        var note = new ObservationNote()
        {
            Id = 3,
            Note = "test note 3",
            NoteType = ObservationNoteType.General,
            Observation = context.Observations.FirstOrDefault()
        };

        // Act
        service.Add(note);
        context.ObservationNotes.Count().ShouldEqual(2);
        context.SaveChanges();

        // Assert
        context.ObservationNotes.Count().ShouldEqual(3);
        Assert.IsType<ObservationNote>(context.ObservationNotes.Where(i => i.Id == 3).FirstOrDefault());
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
            ObservationDateTime = DateTime.Now,
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);

        context.ObservationNotes.Count().ShouldEqual(2);

        var service = new ObservationNoteRepository(context);

        var notes = new List<ObservationNote>
        {
            new()
            {
                Id = 3,
                Note = "test note 3",
                NoteType = ObservationNoteType.General,
                Observation = context.Observations.Where(o => o.ObservationId == 1).FirstOrDefault()
            },
            new()
            {
                Id = 4,
                Note = "test note 4",
                NoteType = ObservationNoteType.General,
                Observation = context.Observations.Where(o => o.ObservationId == 1).FirstOrDefault()
            },
        };

        // Act
        service.AddRange(notes);
        context.ObservationNotes.Count().ShouldEqual(2);
        context.SaveChanges();

        // Assert
        context.ObservationNotes.Count().ShouldEqual(4);
        Assert.IsType<ObservationNote>(context.ObservationNotes.Where(i => i.Id == 1).FirstOrDefault());
        Assert.IsType<ObservationNote>(context.ObservationNotes.Where(i => i.Id == 2).FirstOrDefault());
        Assert.IsType<ObservationNote>(context.ObservationNotes.Where(i => i.Id == 3).FirstOrDefault());
        Assert.IsType<ObservationNote>(context.ObservationNotes.Where(i => i.Id == 4).FirstOrDefault());
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
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);
        context.ObservationNotes.Count().ShouldEqual(2);

        var service = new ObservationNoteRepository(context);

        // Act
        var note = await service.GetAsync(1);

        service.Remove(note);
        context.SaveChanges();

        // Assert
        context.ObservationNotes.Count().ShouldEqual(1);
        context.ObservationNotes.ShouldNotContain(note);
    }

    [Fact]
    public async Task RemoveRange_Should_Remove_Entities()
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
            Notes = new List<ObservationNote>
            {
                new()
                {
                    Id = 1,
                    Note = "test note 1",
                    NoteType = ObservationNoteType.General
                },
                new()
                {
                    Id = 2,
                    Note = "test note 2",
                    NoteType = ObservationNoteType.General,
                }
            }
        };

        context.Observations.Add(ob1);
        context.SaveChanges();
        context.Observations.Count().ShouldEqual(1);
        context.ObservationNotes.Count().ShouldEqual(2);


        var service = new ObservationNoteRepository(context);

        var notes = await service.GetAllAsync();

        // Act
        service.RemoveRange(notes);
        context.SaveChanges();

        // Assert
        context.ObservationPosition.Count().ShouldEqual(0);
    }
}