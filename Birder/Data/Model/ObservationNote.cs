
namespace Birder.Data.Model
{
    public enum ObservationNoteType
    {
        General = 0,
        Habitat = 1,
        // Note plant life, water sources and vegetation conditions, as well as which of the plants the bird is preferring as you observe it.
        Weather = 2,
        // Note temperature, visibility, wind, light level and any weather conditions that affect your observations.Rain, mist, snowfall, accumulated snow, drought and other factors can impact observations.
        Appearance = 3,
        // Take copious notes on the bird's appearance, including the brilliance of plumage, any peculiar markings and any outstanding or unusual features such as missing feathers, leucistic patches or signs of illness. Also record the bird's gender if possible.
        Behaviour = 4,
        // Take notes on what the bird was doing as you observed it.Note general actions and specific reactions to changing conditions, such as the appearance of a predator or how the bird interacts with other birds.Note large actions such as preening, flight patterns and foraging habits as well as small movements such as tail bobs, head cocks or wing stretches.
        Vocalisation = 5
        // If the bird sang or made other sounds during your observation, use mnemonics or descriptions of how it sounded. Also note non-vocal sounds such as wing noises or drumming.
    }

    public class ObservationNote
    {
        public int Id { get; set; }
        public ObservationNoteType NoteType { get; set; }
        public string Note { get; set; }
        public Observation Observation { get; set; }
    }
}
