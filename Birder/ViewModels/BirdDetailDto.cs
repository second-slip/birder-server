using Birder.Data.Model;
using System;

namespace Birder.ViewModels
{
    /// <summary>
    /// A view model containing nearly all Bird information.  Primarily used in the Bird Detail view.
    /// </summary>
    public class BirdDetailDto
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

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public ConservationStatus BirdConservationStatus { get; set; }

        public string BirderStatus { get; set; }
    }
}
