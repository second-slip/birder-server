

namespace Birder.ViewModels;
public class TweetDayDto
{
    public int TweetDayId { get; set; }
    public string SongUrl { get; set; }
    public DateTime DisplayDay { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public int BirdId { get; set; }
    public string Species { get; set; }
    public string EnglishName { get; set; }
}

// public class TweetArchiveDto
// {
//     public int TotalItems { get; set; }
//     public IEnumerable<TweetDayDto> Items { get; set; }
// }