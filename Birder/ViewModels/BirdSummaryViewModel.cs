namespace Birder.ViewModels;

/// <summary>
/// A lightweight viewmodel containing Bird summary info only.  Used for drop down lists and
/// in the observation viewmodel (which just needs to show the species observered.
/// </summary>
public class BirdSummaryDto
{
    public int BirdId { get; set; }

    public string Species { get; set; }

    public string EnglishName { get; set; }

    public string PopulationSize { get; set; }

    public string BtoStatusInBritain { get; set; }

    public string ThumbnailUrl { get; set; }

    public string ConservationStatus { get; set; }

    public string ConservationListColourCode { get; set; }

    public BirderStatus BirderStatus { get; set; }
}