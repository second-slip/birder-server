namespace Birder.MinApiEndpoints;

public class GeneralEndpoints
{
    public static string ServerInfo(ISystemClockService clock)
    {
        var date = clock.GetNow;

        var text = string.Join(
            Environment.NewLine,
            "birder-server API v1.9",
            "ASP.NET 8",
            "https://github.com/second-slip/birder-server",
            $"{date}",
            $"\u00A9 Birder 2020 - {date.Year}");

        return text;
    }
}