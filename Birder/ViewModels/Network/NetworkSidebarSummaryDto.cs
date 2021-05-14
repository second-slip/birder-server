using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class NetworkSidebarSummaryDto
    {
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public IEnumerable<NetworkUserViewModel> SuggestedUsersToFollow { get; set; }
    }
}
