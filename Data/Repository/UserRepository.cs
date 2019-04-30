using Birder.Controllers;
using Birder.Data.Model;
using Birder.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Birder.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            return await _dbContext.Users.Where(e => e.Email.ToUpper() == email.ToUpper()).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserAndNetworkAsyncByUserName(ApplicationUser user)
        {
            return await _dbContext.Users
                         .Include(x => x.Followers)
                             .ThenInclude(x => x.Follower)
                         .Include(y => y.Following)
                             .ThenInclude(r => r.ApplicationUser)
                         .Where(x => x.UserName == user.UserName)
                            .FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserAndNetworkAsyncByUserName(string userName)
        {
            return await _dbContext.Users
                         .Include(x => x.Followers)
                             .ThenInclude(x => x.Follower)
                         .Include(y => y.Following)
                             .ThenInclude(r => r.ApplicationUser)
                         .Where(x => x.UserName == userName)
                         .FirstOrDefaultAsync();
        }

        //public IEnumerable<UserViewModel> GetFollowingList(ApplicationUser user)
        //{
        //    var followingList = from following in user.Following
        //                            select new UserViewModel
        //                            {
        //                                UserName = following.ApplicationUser.UserName,
        //                                ProfileImage = following.ApplicationUser.ProfileImage
        //                            };
        //    return followingList;
        //}

        //public IEnumerable<UserViewModel> GetFollowersList(ApplicationUser user)
        //{
        //    var followerList = from follower in user.Followers
        //                          select new UserViewModel
        //                          {
        //                              UserName = follower.Follower.UserName,
        //                              ProfileImage = follower.Follower.ProfileImage,
        //                          };
        //    return followerList;
        //}

        public IQueryable<NetworkUserViewModel> GetSuggestedBirdersToFollow(ApplicationUser user)
        {
            var followerList = from follower in user.Followers
                               select follower.Follower.UserName;
            var followingList = from following in user.Following
                                select following.ApplicationUser.UserName;

            IEnumerable<string> followersNotBeingFollowed = followerList.Except(followingList);
            //IEnumerable<NetworkUserViewModel> suggestedBirders = new List<NetworkUserViewModel>();

            if (followersNotBeingFollowed.Count() != 0)
            {
                var suggestedBirders = from users in _dbContext.Users
                           .Where(users => followersNotBeingFollowed.Contains(users.UserName))
                                       select new NetworkUserViewModel
                                       {
                                           UserName = users.UserName,
                                           ProfileImage = users.ProfileImage,
                                           IsFollowing = users.Following.Any(cus => cus.ApplicationUser.UserName == users.UserName)
            };
                return suggestedBirders;
            }
            else
            {
                var suggestedBirders = from users in _dbContext.Users
                                   .Where(users => !followingList.Contains(users.UserName) && users.UserName != user.UserName)
                                   select new NetworkUserViewModel
                                   {
                                       UserName = users.UserName,
                                       ProfileImage = users.ProfileImage,
                                       IsFollowing = users.Following.Any(cus => cus.ApplicationUser.UserName == users.UserName)
                                   };
                return suggestedBirders;
            }
            
            ////return suggestedBirders.ToList();
        }

        //public List<NetworkUserViewModel> GetSuggestedBirdersToFollow(ApplicationUser user, string searchCriterion)
        public IQueryable<NetworkUserViewModel> GetSuggestedBirdersToFollow(ApplicationUser user, string searchCriterion)
        {
            var followingList = from following in user.Following
                                select following.ApplicationUser.UserName;

            //IEnumerable<NetworkUserViewModel> suggestedBirders = new List<NetworkUserViewModel>();
            //var suggestedBirders = new List<NetworkUserViewModel>();
            var suggestedBirders = from users in _dbContext.Users
                               where (users.UserName.ToUpper().Contains(searchCriterion.ToUpper()) && !followingList.Contains(users.UserName) && users.UserName != user.UserName) // .Contains(users.UserName) // != user.UserName)
                               select new NetworkUserViewModel
                               {
                                   UserName = users.UserName,
                                   ProfileImage = users.ProfileImage,
                                   IsFollowing = users.Following.Any(cus => cus.ApplicationUser.UserName == user.UserName)
                               };
        

            return suggestedBirders;
        }



        public void Follow(ApplicationUser loggedinUser, ApplicationUser userToFollow)
        {
            userToFollow.Followers.Add(new Network { Follower = loggedinUser });
            _dbContext.SaveChanges();
        }

        public void UnFollow(ApplicationUser loggedinUser, ApplicationUser userToUnfollow)
        {
            loggedinUser.Following.Remove(userToUnfollow.Followers.FirstOrDefault());
            _dbContext.SaveChanges();
        }

        // ToDo: DRY - this method is repeated verbatim in the observation repository
        public IQueryable<Observation> GetUsersObservationsList(string userId)
        {
            var observations = _dbContext.Observations
                .Where(o => o.ApplicationUserId == userId)
                    .Include(au => au.ApplicationUser)
                    .Include(b => b.Bird)
                    .Include(ot => ot.ObservationTags)
                        .ThenInclude(t => t.Tag)
                    .OrderByDescending(d => d.ObservationDateTime)
                    .AsNoTracking();
            return observations;
        }

        // ToDo: DRY - this method is repeated verbatim in the observation repository
        public async Task<int> UniqueSpeciesCount(ApplicationUser user)
        {
            return await (from observations in _dbContext.Observations
                          where (observations.ApplicationUserId == user.Id)
                          select observations.BirdId).Distinct().CountAsync();
        }
    }
}
