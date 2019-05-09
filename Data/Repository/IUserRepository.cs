using Birder.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserAndNetworkAsyncByUserName(string userName);
        void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow);
        void UnFollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow);
        Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(ApplicationUser user, IEnumerable<string> followersNotBeingFollowed);
        Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(ApplicationUser user, IEnumerable<string> followingList);
        Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(ApplicationUser user, string searchCriterion, IEnumerable<string> followingList);
    }
}
