using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.ViewModels
{
    public class BirdSummaryViewModel
    {
        public int BirdId { get; set; }

        public string Class { get; set; }

        public string Order { get; set; }

        public string Family { get; set; }

        public string Genus { get; set; }

        public string Species { get; set; }

        public string EnglishName { get; set; }

        public string InternationalName { get; set; }

        public string Category { get; set; } //Primary only

        //public string PopulationSize { get; set; }


        //public string BtoStatusInBritain { get; set; }

        // validation removed - causes faults on Model.Validate if url is empty
        //[Url]
        public string ThumbnailUrl { get; set; }

        //[Url]
        //public string SongUrl { get; set; }

        //public DateTime CreationDate { get; set; }

        //public DateTime LastUpdateDate { get; set; }

        //

        //public int ConserverationStatusId { get; set; }

        //public int BritishStatusId { get; set; }

        //public ICollection<Observation> Observations { get; set; }

        public string ConserverationStatus { get; set; }

        public string BirderStatus { get; set; }
        //public BirderStatus BirderStatus { get; set; }

        //public ICollection<TweetDay> TweetDay { get; set; }
    }
}
