namespace Birder.ViewModels
{
    // public sealed record TopObservationsViewModel
    // {
    //     public int BirdId { get; init; }
    //     public string Name { get; init; }
    //     public int Count { get; init; }
    // }

    public readonly record struct TopObservationsViewModel(int BirdId, string Name, int Count);
}