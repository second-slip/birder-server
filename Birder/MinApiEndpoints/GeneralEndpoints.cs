namespace Birder.MinApiEndpoints;

public class GeneralEndpoints
{
    public static string ServerInfo(ISystemClockService clock)
    {
        var date = clock.GetNow;

        var text = string.Join(
            Environment.NewLine,
            "birder-server API v2.0.0",
            "ASP.NET 10",
            "https://github.com/second-slip/birder-server",
            $"{date}",
            $"\u00A9 Birder 2020 - {date.Year}");

        return text;
    }
}