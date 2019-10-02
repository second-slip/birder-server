using Birder.Data.Model;

namespace Birder.Data.Repository
{
    public interface INetworkRepository
    {
        //Task<ApplicationUser> GetUserAndNetworkAsync(string userName);
        void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow);
        void UnFollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow);
        //Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(ApplicationUser user, IEnumerable<string> followersNotBeingFollowed);
        //Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(ApplicationUser user, IEnumerable<string> followingList);
        //Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(ApplicationUser user, string searchCriterion, IEnumerable<string> followingList);
    }
}
