using System.ComponentModel.DataAnnotations;

namespace Birder.Data.Model;

public class Network
{
    [Key]
    public int NetworkId { get; set; } 
    // added primary key, to make ApplicationUserId/FollowerId unique index
    // otherwise breaking change since EF6: "The property 'Network.FollowerId' is part of a key and so cannot be modified or marked as modified. To change the principal of an existing entity with an identifying foreign key, first delete the dependent and invoke 'SaveChanges', and then associate the dependent with the new principal."
    public ApplicationUser ApplicationUser { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser Follower { get; set; }
    public string FollowerId { get; set; }
}