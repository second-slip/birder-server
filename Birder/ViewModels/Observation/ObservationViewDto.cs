using System.Collections.Generic;

namespace Birder.ViewModels;
public class ObservationsPagedDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<ObservationViewDto> Items { get; set; }
    }

public class ObservationViewDto
{
    public int ObservationId { get; set; }
    public int Quantity { get; set; }
    public DateTime ObservationDateTime { get; set; }
    public int BirdId { get; set; }
    public string Species { get; set; }
    public string EnglishName { get; set; }
    public string Username { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}