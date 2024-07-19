using System.Text;
using System.Web;


namespace DSharp.NetCore.Internal;

internal static class General
{
    public static string Line(int length = 80)
    {
        return new string('-', length);
    }


    public static string NiceUrlParameters(string urlValues)
    {
        var sb = new StringBuilder();
        var items = HttpUtility.UrlDecode(urlValues).Split('&');
        foreach (var item in items)
        {
            if (item.StartsWith("password"))
            {
                sb.AppendLine("password=********");
                continue;
            }

            sb.AppendLine(item);
        }
        return sb.ToString();

    }
}
