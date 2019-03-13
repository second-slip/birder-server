using Birder.Data.Model;
using System;
using System.Collections.Generic;

namespace Birder.ViewModels
{
    /// <summary>
    /// A view model containing nearly all Bird information.  Primarily used in the Bird Detail view.
    /// </summary>
    public class BirdDetailViewModel
    {
        public int BirdId { get; set; }

        public string Class { get; set; }

        public string Order { get; set; }

        public string Family { get; set; }

        public string Genus { get; set; }

        public string Species { get; set; }

        public string EnglishName { get; set; }

        public string InternationalName { get; set; }

        public string Category { get; set; }

        public string PopulationSize { get; set; }

        public string BtoStatusInBritain { get; set; }

        public string ThumbnailUrl { get; set; }

        public string SongUrl { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        //public int ConserverationStatusId { get; set; }

        // lazy load observations
        //public ICollection<Observation> Observations { get; set; }

        public ConserverationStatus BirdConserverationStatus { get; set; }

        public BirderStatus BirderStatus { get; set; }

        //public ICollection<TweetDay> TweetDay { get; set; }
    }
}
