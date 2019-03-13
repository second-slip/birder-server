namespace Birder.ViewModels
{
    /// <summary>
    /// A lightweight viewmodel containing Bird summary info only.  Used for drop down lists and
    /// in the observation viewmodel (which just needs to show the species observered.
    /// </summary>
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

        public string Category { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ConserverationStatus { get; set; }

        public string BirderStatus { get; set; }
    }
}
