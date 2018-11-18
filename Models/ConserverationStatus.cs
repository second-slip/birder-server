using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Birder.Models
{
    public class ConserverationStatus
    {
        [Key]
        public int ConserverationStatusId { get; set; }

        [Required]
        public string ConservationStatus { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public ICollection<Bird> Birds { get; set; }
    }
}
