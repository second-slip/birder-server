namespace Birder.ViewModels
{
    public class ObservationNoteDto
    {
        public int Id { get; set; }
        public string NoteType { get; set; }
        public string Note { get; set; }
        public int ObervationId { get; set; }
        //public Observation Observation { get; set; }
    }
}
