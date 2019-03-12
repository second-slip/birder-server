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
        [Display(Name = "Scientific Name")]
        public string Species { get; set; }

        [Required]
        [Display(Name = "English Vernacular Name")]
        public string EnglishName { get; set; }

        [Display(Name = "International Name")]
        public string InternationalName { get; set; }

        public string Category { get; set; } //Primary only

        //
        // - https://www.bto.org/about-birds/birdfacts/british-list
        // - This list includes 603 species (as at 1 January 2017)
        //
        [Display(Name = "BTO Population Size in Britain")]
        public string PopulationSize { get; set; }

        [Display(Name = "BTO Status in Britain")]
        public string BtoStatusInBritain { get; set; }

        // validation removed - causes faults on Model.Validate if url is empty
        //[Url]
        public string ThumbnailUrl { get; set; }

        //[Url]
        public string SongUrl { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        //

        public int ConserverationStatusId { get; set; }

        //public int BritishStatusId { get; set; }

        public ICollection<Observation> Observations { get; set; }

        public ConserverationStatus BirdConserverationStatus { get; set; }

        public BirderStatus BirderStatus { get; set; }

        public ICollection<TweetDay> TweetDay { get; set; }
    }
}
