using Birder.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.ViewModels
{
    public class ObservationViewModel
    {
        //ToDo: male/female/juvenile? Or is it too much?
        //[Key]
        public int ObservationId { get; set; }

        //public Geography LocationGeoTest { get; set; } ---> Not supported in EF Core 2.0
        public double LocationLatitude { get; set; }

        public double LocationLongitude { get; set; }

        //[Range(1, 1000, ErrorMessage = "The value must be greater than 0")]
        //[Display(Name = "Individuals")]
        public int Quantity { get; set; }

        //[Display(Name = "General notes")]
        public string NoteGeneral { get; set; }

        //[Display(Name = "Habitat notes")]
        public string NoteHabitat { get; set; }
        // Note plant life, water sources and vegetation conditions, as well as which of the plants the bird is preferring as you observe it.

        //[Display(Name = "Weather notes")]
        public string NoteWeather { get; set; }
        // Note temperature, visibility, wind, light level and any weather conditions that affect your observations.Rain, mist, snowfall, accumulated snow, drought and other factors can impact observations.

        //[Display(Name = "Appearance notes")]
        public string NoteAppearance { get; set; }
        // Take copious notes on the bird's appearance, including the brilliance of plumage, any peculiar markings and any outstanding or unusual features such as missing feathers, leucistic patches or signs of illness. Also record the bird's gender if possible.

        //[Display(Name = "Behaviour notes")]
        public string NoteBehaviour { get; set; }
        // Take notes on what the bird was doing as you observed it.Note general actions and specific reactions to changing conditions, such as the appearance of a predator or how the bird interacts with other birds.Note large actions such as preening, flight patterns and foraging habits as well as small movements such as tail bobs, head cocks or wing stretches.

        //[Display(Name = "Vocalisation notes")]
        public string NoteVocalisation { get; set; }
        // If the bird sang or made other sounds during your observation, use mnemonics or descriptions of how it sounded. Also note non-vocal sounds such as wing noises or drumming.

        //public bool HasPhotos { get; set; }

        //public PrivacyLevel SelectedPrivacyLevel { get; set; }

        [Required]
        //[Display(Name = "Date/Time")]
        [DataType(DataType.DateTime)]
        public DateTime ObservationDateTime { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        //[Display(Name = "Observed species")]
        public int BirdId { get; set; }
        //public string ApplicationUserId { get; set; }

        public BirdSummaryViewModel Bird { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        public UserViewModel User { get; set; }
        //public ICollection<ObservationTag> ObservationTags { get; set; }
        //public ICollection<Photograph> Photographs { get; set; }
    }
}
