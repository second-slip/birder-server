using System;
using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model
{
    public class TweetDay
    {
        [Key]
        public int TweetDayId { get; set; }

        public DateTime DisplayDay { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public int BirdId { get; set; }

        public Bird Bird { get; set; }
    }
}
