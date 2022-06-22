namespace Birder.Data.Model;

public class Network
{
    public ApplicationUser ApplicationUser { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser Follower { get; set; }
    public string FollowerId { get; set; }
}