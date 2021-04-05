using Birder.ViewModels;
using System.Collections.Generic;

namespace Birder.Tests.Controller
{
    public static class FeedTestHelpers
    {
        public static ObservationFeedPagedDto GetModel(int totalItems)
        {
            var model = new ObservationFeedPagedDto()
            {
                TotalItems = totalItems,
                Items = new List<ObservationFeedDto>() { new ObservationFeedDto() }
            };

            return model;
        }
    }
}
