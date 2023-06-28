namespace Birder.MinApiEndpoints;

public class GeneralEndpoints
{
    public static string ServerInfo()
    {
        var text = string.Join(
            Environment.NewLine,
            "birder-server API",
            "https://github.com/winthorpecross/birder-server",
            $"{DateTime.Now}",
            $"\u00A9 Birder {DateTime.Now.Year}");

        return text;
    }
}