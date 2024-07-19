using System.Text;

namespace DSharp.NetCore;

internal static class Message
{
    public static string Time => $"{DateTime.Now:HH:mm:ss.fff}";

    public static string Normal(string message, string body, Level level = Level.None)
    {
        if (Log.Level < level)
            return string.Empty;

        var logCode = GetLogTypeCode(level);
        var m = $"{Time}{logCode}";
        if (Check.IsNotEmpty(message))
            m = $"{m} {message}";

        Console.WriteLine(m);

        if (Check.IsEmpty(body))
            return m;

        return $"{m}{Environment.NewLine}{body}";
    }

    public static string Exception(Exception exception, string body)
    {
        var logCode = GetLogTypeCode(Level.Error);
        var m = $"{Time}{logCode} {exception.Message}";

        if (Check.IsNotEmpty(body))
            m = $"{m}{Environment.NewLine}{body}";

        var simple = Log.Level < Level.Debug;

        return $"{m}{Environment.NewLine}{ExtensionTree(exception, simple)}";
    }


    public static string GetLogTypeCode(Level level)
    {
        return level switch
        {
            Level.Debug => " [DBG]",
            Level.Information => " [INF]",
            Level.Verbose => " [VRB]",
            Level.Warning => " [WRN]",
            Level.Error => " [ERR]",
            Level.Process => " [PRC]",
            Level.Trace => " [TRC]",
            _ => string.Empty,
        };
    }
    private static string ExtensionTree(Exception ex, bool simple)
    {
        var sb = new StringBuilder();
        sb.AppendLine(ex.Message);
        if (!simple)
            sb.AppendLine(ex.StackTrace);

        if (ex.InnerException != null)
            sb.AppendLine(ExtensionTree(ex.InnerException, simple));

        return sb.ToString();
    }
}