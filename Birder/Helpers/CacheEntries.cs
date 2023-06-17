namespace Birder.Helpers;

public enum CacheEntries
{
    ObservationsList = 0,
    BirdsSummaryList = 1,
    ObservationsSummary = 2,
    ShowcaseObservations = 3
}

public class CacheEntryKeys
{
    public const string BirdThumbUrl = "BirdThumbUrl";
    //public const int ListItems = 1001;
}