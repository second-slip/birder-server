using TestSupport.EfHelpers;

namespace Birder.Tests.Services;

public class TweetDataSericeTests
{
    // GetTweetArchiveAsync(int pageIndex, int pageSize, DateTime date)
    [Theory]
    [InlineData(0, 1, 30)]
    [InlineData(1, 5, 30)]
    [InlineData(2, 15, 30)]
    [InlineData(3, 20, 70)]
    public async Task DataService_Returns_Correct_Items(int pageIndex, int pageSize, int totalItems)
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        for (int i = 1; i <= totalItems; i++)
        {
            context.TweetDays.Add(GetTweetObject(i));
        }
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(totalItems);

        // Act
        var service = new TweetDataService(context);
        var model = await service.GetTweetArchiveAsync(pageIndex, pageSize, DateTime.Today);

        // Assert
        Assert.IsAssignableFrom<IEnumerable<TweetDayDto>>(model);
        Assert.Equal(pageSize, model.Count());
    }

    [Theory]
    [InlineData(0, 0, 30)]
    [InlineData(1, 0, 30)]
    [InlineData(2, 0, 30)]
    [InlineData(3, 0, 70)]
    public async Task DataService_Returns_Default_Items_When_PageSize_Is_Zero(int pageIndex, int pageSize, int totalItems)
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        for (int i = 1; i <= totalItems; i++)
        {
            context.TweetDays.Add(GetTweetObject(i));
        }
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(totalItems);

        // Act
        var service = new TweetDataService(context);
        var model = await service.GetTweetArchiveAsync(pageIndex, pageSize, DateTime.Today);

        // Assert
        var expectedPageSize = 10; // default set in ApplyPaging
        Assert.Equal(expectedPageSize, model.Count());
        Assert.IsAssignableFrom<IEnumerable<TweetDayDto>>(model);
    }


    // ....................................................
    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async Task Returns_Tweet_Of_Day_With_Todays_Date_When_Available(int totalItems)
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        for (int i = 1; i <= totalItems; i++)
        {
            context.TweetDays.Add(GetTweetObject(i));
        }
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(totalItems);
        //
        var expectedId = 9999;
        context.TweetDays.Add(GetTweetObjectWithTodayDisplayDate(expectedId));
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(totalItems + 1);

        // Act
        var service = new TweetDataService(context);
        var model = await service.GetTweetOfTheDayAsync(DateTime.Today);

        // Assert
        Assert.IsType<TweetDayDto>(model);
        Assert.Equal(expectedId, model.TweetDayId);
        Assert.Equal(DateTime.Today, model.DisplayDay);
    }

        [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public async Task Returns_ANY_Tweet_Of_Day_When_Todays_Date_NOT_Available(int totalItems)
    {
        // Arrange
        var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
        using var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        for (int i = 1; i <= totalItems; i++)
        {
            context.TweetDays.Add(GetTweetObject(i));
        }
        context.SaveChanges();
        context.Birds.Count().ShouldEqual(totalItems);

        // Act
        var service = new TweetDataService(context);
        var model = await service.GetTweetOfTheDayAsync(DateTime.Today);

        // Assert
        Assert.IsType<TweetDayDto>(model);
        Assert.NotEqual(DateTime.Today, model.DisplayDay);
    }



    private static Bird GetBirdObject(int id)
    {
        Random r = new Random();

        return new Bird()
        {
            BirdId = id,
            Class = $"Class {id}",
            Order = $"Order {id}",
            Family = $"Family {id}",
            Genus = $"Genus {id}",
            Species = $"Species {id}",
            EnglishName = $"Name {id}",
            ConservationStatusId = r.Next(1, 3),
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now
        };
    }

    private static TweetDay GetTweetObject(int id)
    {
        return (new TweetDay()
        {
            TweetDayId = id,
            SongUrl = $"url {id}",
            DisplayDay = DateTime.Today.AddDays(-id),
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            BirdId = id,
            Bird = GetBirdObject(id)
        });
    }

    private static TweetDay GetTweetObjectWithTodayDisplayDate(int id)
    {
        return (new TweetDay()
        {
            TweetDayId = id,
            SongUrl = $"url {id}",
            DisplayDay = DateTime.Today,
            CreationDate = DateTime.Now,
            LastUpdateDate = DateTime.Now,
            BirdId = id,
            Bird = GetBirdObject(id)
        });
    }
}