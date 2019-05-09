using Birder.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public interface IUserRepository
    {
        //Task<ApplicationUser> GetUserByEmail(string email);
        //Task<ApplicationUser> GetUserAndNetworkAsyncByUserName(ApplicationUser user);
        Task<ApplicationUser> GetUserAndNetworkAsyncByUserName(string userName);
        //IEnumerable<UserViewModel> GetFollowingList(ApplicationUser user);
        //IEnumerable<UserViewModel> GetFollowersList(ApplicationUser user);
        void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow);
        void UnFollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow);
        //IQueryable<NetworkUserViewModel> GetSuggestedBirdersToFollow(ApplicationUser user); //, IEnumerable<string> followersNotBeingFollowed);

        Task<IEnumerable<ApplicationUser>> GetFollowersNotFollowedAsync(ApplicationUser user, IEnumerable<string> followersNotBeingFollowed);
        Task<IEnumerable<ApplicationUser>> GetSuggestedBirdersToFollowAsync(ApplicationUser user, IEnumerable<string> followingList);

        Task<IEnumerable<ApplicationUser>> SearchBirdersToFollowAsync(ApplicationUser user, string searchCriterion, IEnumerable<string> followingList);

        //IQueryable<NetworkUserViewModel> GetSuggestedBirdersToFollow(ApplicationUser user, string searchCriterion);
        //IQueryable<Observation> GetUsersObservationsList(string userId);
        //Task<int> UniqueSpeciesCount(ApplicationUser user);
    }
}
