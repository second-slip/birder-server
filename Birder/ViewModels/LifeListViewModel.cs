namespace Birder.ViewModels
{
    public readonly record struct LifeListViewModel
    {
        public int BirdId { get; init; }
        public string EnglishName { get; init; }
        public string Species { get; init; }
        public string PopulationSize { get; init; }
        public string BtoStatusInBritain { get; init; }
        public string ConservationStatus { get; init; }
        public string ConservationListColourCode { get; init; }
        public int Count { get; init; }
    }
}
