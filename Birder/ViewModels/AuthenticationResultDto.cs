namespace Birder.ViewModels
{
    public class AuthenticationResultDto
    {
        public string AuthenticationToken { get; set; } = null!;

        public AuthenticationFailureReason FailureReason { get; set; } = AuthenticationFailureReason.None;
    }

    public enum AuthenticationFailureReason
    {
        None = 0,
        EmailConfirmationRequired = 1,
        LockedOut = 2,
        Other = 3
    }
}
