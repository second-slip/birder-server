namespace Birder.MinApiEndpoints;

public class GeneralEndpoints
{
    public static string ServerInfo(ISystemClockService clock)
    {
        var date = clock.GetNow;

        var text = string.Join(
            Environment.NewLine,
            "birder-server API v1.8",
            "https://github.com/andrew-stuart-cross/birder-server",
            $"{date}",
            $"\u00A9 Birder {date.Year}");

        return text;
    }
}