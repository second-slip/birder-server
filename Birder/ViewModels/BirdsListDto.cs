using System.Collections.Generic;

namespace Birder.ViewModels;

public class BirdsListDto
{
    public int TotalItems { get; set; }
    public IEnumerable<BirdSummaryDto> Items { get; set; }
}
