using System;

namespace Birder.ViewModels
{
    public class TweetDayViewModel
    {
        public int TweetDayId { get; set; }

        public string SongUrl { get; set; }

        public DateTime DisplayDay { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public BirdSummaryViewModel Bird { get; set; }
    }
}
