using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model
{
    public class Bird
    {
        [Key]
        public int BirdId { get; set; }

        [Required]
        public string Class { get; set; }

        [Required]
        public string Order { get; set; }

        [Required]
        public string Family { get; set; }

        [Required]
        public string Genus { get; set; }

        [Required]
        public string Species { get; set; }

        [Required]
        public string EnglishName { get; set; }

        public string InternationalName { get; set; }

        public string Category { get; set; } //Primary only

        //
        // - https://www.bto.org/about-birds/birdfacts/british-list
        // - This list includes 603 species (as at 1 January 2017)
        //
        // BTO Population Size in Britain
        public string PopulationSize { get; set; }

        public string BtoStatusInBritain { get; set; }

        // validation removed - causes faults on Model.Validate if url is empty
        //[Url]
        public string ThumbnailUrl { get; set; }

        //[Url]
        //public string SongUrl { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        //

        public int ConservationStatusId { get; set; }

        public ICollection<Observation> Observations { get; set; }

        public ConservationStatus BirdConservationStatus { get; set; }

        public BirderStatus BirderStatus { get; set; }

        public ICollection<TweetDay> TweetDay { get; set; }
    }
}
