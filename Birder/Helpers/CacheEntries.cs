namespace Birder.Helpers
{
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
        //public const int GetItem = 1002;
        //public const int InsertItem = 1003;
        //public const int UpdateItem = 1004;
        //public const int DeleteItem = 1005;

        //public const int InvalidModelState = 4010;

        //public const int GetItemNotFound = 4000;
        //public const int GetListNotFound = 4001;
        //public const int UpdateItemNotFound = 4002;
    }
}
