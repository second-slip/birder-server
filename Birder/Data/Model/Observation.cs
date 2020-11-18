using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model
{
    public class Observation
    {
        //ToDo: male/female/juvenile? Or is it too much?
        [Key]
        public int ObservationId { get; set; }

        public double LocationLatitude { get; set; }

        public double LocationLongitude { get; set; }

        //[Range(1, 1000, ErrorMessage = "The value must be greater than 0")]
        [Range(1, int.MaxValue, ErrorMessage = "The observation count should be at least one individual")]
        public int Quantity { get; set; }

        public string NoteGeneral { get; set; }

        public string NoteHabitat { get; set; }
        // Note plant life, water sources and vegetation conditions, as well as which of the plants the bird is preferring as you observe it.

        public string NoteWeather { get; set; }
        // Note temperature, visibility, wind, light level and any weather conditions that affect your observations.Rain, mist, snowfall, accumulated snow, drought and other factors can impact observations.

        public string NoteAppearance { get; set; }
        // Take copious notes on the bird's appearance, including the brilliance of plumage, any peculiar markings and any outstanding or unusual features such as missing feathers, leucistic patches or signs of illness. Also record the bird's gender if possible.

        public string NoteBehaviour { get; set; }
        // Take notes on what the bird was doing as you observed it.Note general actions and specific reactions to changing conditions, such as the appearance of a predator or how the bird interacts with other birds.Note large actions such as preening, flight patterns and foraging habits as well as small movements such as tail bobs, head cocks or wing stretches.

        public string NoteVocalisation { get; set; }
        // If the bird sang or made other sounds during your observation, use mnemonics or descriptions of how it sounded. Also note non-vocal sounds such as wing noises or drumming.

        public bool HasPhotos { get; set; }

        public PrivacyLevel SelectedPrivacyLevel { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ObservationDateTime { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "You must choose the bird species you observed")]
        public int BirdId { get; set; }
        public string ApplicationUserId { get; set; }
        public int ObservationPositionId { get; set; }
        //
        public Bird Bird { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ObservationPosition Position { get; set; }
        public ICollection<ObservationTag> ObservationTags { get; set; }
        //public ICollection<Photograph> Photographs { get; set; }
    }
}
