using System.Collections.Generic;

namespace Birder.ViewModels
{
    public class UserNetworkDto
    {
        public IEnumerable<FollowerViewModel> Followers { get; set; }
        public IEnumerable<FollowingViewModel> Following { get; set; }
    }
}
