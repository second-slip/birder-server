using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.ViewModels
{
    public class TweetDayViewModel
    {
        public int TweetDayId { get; set; }

        public DateTime DisplayDay { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public BirdSummaryViewModel Bird { get; set; }
    }
}
