using System;
using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class ObservationFeedPagedDto
    {
        public int TotalItems { get; set; }
        public IEnumerable<ObservationFeedDto> Items { get; set; }
        public ObservationFeedFilter ReturnFilter { get; set; }
    }

    public class ObservationFeedDto
    {
        //First Time? .Any() ????
        public int ObservationId { get; set; }
        public int Quantity { get; set; }
        public DateTime ObservationDateTime { get; set; }
        public int BirdId { get; set; }
        public string Species { get; set; }
        public string EnglishName { get; set; }
        public string ThumbnailUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormattedAddress { get; set; }
        public string ShortAddress { get; set; }
        public string Username { get; set; }
        public int NotesCount { get; set; }
        //public int PhotosCount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
