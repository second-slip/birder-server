using System;
using System.Collections.Generic;

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

    public class TweetArchiveDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<TweetDayViewModel> Items { get; set; }
    }
}
