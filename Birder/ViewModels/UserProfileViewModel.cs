namespace Birder.ViewModels;

public class UserProfileViewModel
{
    public NetworkUserViewModel User { get; set; }
    public DateTime RegistrationDate { get; set; }
    public ObservationAnalysisViewModel ObservationCount { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}