using System.Net;
using System.Net.Http.Headers;
using System.Text;

using DSharp.NetCore.Internal;

namespace DSharp.NetCore;

/// <summary>
/// 
/// </summary>
public static class Log
{
    private static string _folder = @"D:\Logs";
    private static string _name = SafeName;
    private static int _contentSize = 5000;


    /// <summary>
    /// Maximum size of the content
    /// </summary>
    public static int ContentSize
    {
        get => _contentSize;
        set
        {
            _contentSize = value;
            if (_contentSize < 2000)
                _contentSize = 2000;
        }
    }

    /// <summary>
    /// The name of the application
    /// </summary>
    public static string Name
    {
        get => _name;
        set
        {
            _name = value;
            FolderExists = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static bool Exists()
    {
        var filename = GetFileName();
        return File.Exists(filename);
    }


    internal static string SafeName => "DSharp.NetCore.Logger";

    internal static bool FolderExists;


    internal static string GetFileName()
    {
        var folder = AbsoluteFolder();

        if (!Directory.Exists(folder))
        {
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch
            {
                return string.Empty;
            }

        }
        FolderExists = true;

        return Path.Combine(folder, FileName);
    }


    internal static string AbsoluteFolder()
    {
        var name = Name;
        if (Check.IsEmpty(name))
            name = SafeName;

        return Path.Combine(Folder, name, $"{DateTime.Today:yyyy-MM}");
    }

    /// <summary>
    /// 
    /// </summary>
    public static Level Level { get; set; } = Level.None;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    public static void SetLevel(string level)
    {
        Level = Get(level);
    }

    private static Level Get(string name)
    {
        if (name.Length == 0)
            return Level.None;

        return name.ToUpper()[0] switch
        {
            'V' => Level.Verbose,
            'D' => Level.Debug,
            'W' => Level.Warning,
            'E' => Level.Error,
            'T' => Level.Trace,
            'P' => Level.Process,
            'I' => Level.Information,
            _ => Level.None
        };
    }

    /// <summary>
    /// 
    /// </summary>
    public static string FileName => $"{DateTime.Today:yyyy.MM.dd}.log";

    /// <summary>
    /// The folder where the log should be created
    /// </summary>
    public static string Folder
    {
        get => _folder;
        set
        {
            _folder = Check.IsNotEmpty(value) ? value : @"D:\Logs";
            FolderExists = false;
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <param name="value"></param>
    /// <param name="statusCode"></param>
    public static void Response(HttpMethod method, Uri uri, string value, HttpStatusCode statusCode)
    {
        try
        {
            Debug($"{method}-RESPONSE {uri} {(int)statusCode} {statusCode} - {Safe.Data(value, ContentSize)}{General.Line()}");
        }
        catch (Exception ex)
        {
            Exception(ex);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <param name="value"></param>
    /// <param name="statusCode"></param>
    public static async Task ResponseAsync(HttpMethod method, Uri uri, string value, HttpStatusCode statusCode)
    {
        try
        {
            await DebugAsync($"{method}-RESPONSE {uri} {(int)statusCode} {statusCode} - {Safe.Data(value, ContentSize)}{General.Line()}");
        }
        catch (Exception ex)
        {
            await ExceptionAsync(ex);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="headers"></param>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <param name="content"></param>
    /// <param name="value"></param>
    public static void Request(HttpRequestHeaders headers, HttpMethod method, Uri uri, HttpContent? content, string? value)
    {
        try
        {
            if (Level >= Level.Trace)
            {
                var text = string.Empty;
                if (content != null)
                    text = content.ReadAsStringAsync().Result;

                Trace(PrivateTraceLogRequest(headers, method, uri, text));
                return;
            }

            Debug($"{method}-REQUEST {uri}{value}");
        }
        catch (Exception ex)
        {
            Exception(ex);
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="headers"></param>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <param name="content"></param>
    /// <param name="value"></param>
    public static async Task RequestAsync(HttpRequestHeaders headers, HttpMethod method, Uri uri, HttpContent? content, string? value)
    {
        try
        {
            if (Level >= Level.Trace)
            {
                var text = string.Empty;
                if (content != null)
                    text = await content.ReadAsStringAsync();

                await TraceAsync(PrivateTraceLogRequest(headers, method, uri, text));
            }

            await DebugAsync($"{method}-REQUEST {uri}{value}");
        }
        catch (Exception ex)
        {
            await ExceptionAsync(ex);
        }

    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Exception(Exception exception, string body = "") => WriteRaw(Message.Exception(exception, body), Level.Error);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task ExceptionAsync(Exception exception, string body = "") => await WriteRawAsync(Message.Exception(exception, body), Level.Error);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static void Exception(Exception exception, HttpMethod method, Uri uri)
    {
        Exception(exception, $"{method} {uri}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static async Task ExceptionAsync(Exception exception, HttpMethod method, Uri uri)
    {
        await ExceptionAsync(exception, $"{method} {uri}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Information(string message, string body = "") => WriteRaw(Message.Normal(message, body, Level.Information), Level.Information);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task InformationAsync(string message, string body = "") => await WriteRawAsync(Message.Normal(message, body, Level.Information), Level.Information);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Process(string message, string body = "") => WriteRaw(Message.Normal(message, body, Level.Process), Level.Process);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task ProcessAsync(string message, string body = "") => await WriteRawAsync(Message.Normal(message, body, Level.Process), Level.Process);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Debug(string message, string body = "") => WriteRaw(Message.Normal(message, body, Level.Debug), Level.Debug);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task DebugAsync(string message, string body = "") => await WriteRawAsync(Message.Normal(message, body, Level.Debug), Level.Debug);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Verbose(string message, string body = "") => WriteRaw(Message.Normal(message, body, Level.Verbose), Level.Verbose);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task VerboseAsync(string message, string body = "") => await WriteRawAsync(Message.Normal(message, body, Level.Verbose), Level.Verbose);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Trace(string message, string body = "") => WriteRaw(Message.Normal(message, body, Level.Trace), Level.Trace);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task TraceAsync(string message, string body = "") => await WriteRawAsync(Message.Normal(message, body, Level.Trace), Level.Trace);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Warning(string message, string body = "") => WriteRaw(Message.Normal(message, body, Level.Warning), Level.Warning);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task WarningAsync(string message, string body = "") => await WriteRawAsync(Message.Normal(message, body, Level.Warning), Level.Warning);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static void Error(string message, string body = "") => WriteRaw(Message.Normal(message, body, Level.Error), Level.Error);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static async Task ErrorAsync(string message, string body = "") => await WriteRawAsync(Message.Normal(message, body, Level.Error), Level.Error);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static void Write(string message, string body = "", Level level = Level.Information) => WriteRaw(Message.Normal(message, body), level);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="body"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static async Task WriteAsync(string message, string body = "", Level level = Level.Information) => await WriteRawAsync(Message.Normal(message, body), level);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static void WriteRaw(string message, Level level)
    {
        if (Level == 0)
            return;

        if (level > Level)
            return;

        if (Check.IsEmpty(message))
            return;

        var file = GetFileName();
        if (Check.IsEmpty(file))
            return;

        TryAppendToLogFile(file, message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public static async Task WriteRawAsync(string message, Level level)
    {
        if (Level == 0)
            return;

        if (level > Level)
            return;

        if (Check.IsEmpty(message))
            return;

        var file = GetFileName();
        if (Check.IsEmpty(file))
            return;

        await TryAppendToLogFileAsync(file, message);
    }

    // ---------------------------------
    //  Private
    // ---------------------------------

    private static void TryAppendToLogFile(string filename, string message, bool retry = false)
    {
        try
        {
            AppendAllText(filename, message + Environment.NewLine);
        }
        catch
        {
            if (retry)
                return;

            Task.Delay(100);
            TryAppendToLogFile(filename, message, true);
        }
    }

    private static async Task TryAppendToLogFileAsync(string filename, string message, bool retry = false)
    {
        try
        {
            await AppendAllTextAsync(filename, message + Environment.NewLine);
        }
        catch
        {
            if (retry)
                return;
            await Task.Delay(100);
            await TryAppendToLogFileAsync(filename, message, true);
        }
    }

    private static Encoding Utf8NoBom { get; } = new UTF8Encoding(false, true);

    private static async Task AppendAllTextAsync(string path, string? contents)
    {
        await AppendAllTextAsync(path, contents, Utf8NoBom);
    }

    private static void AppendAllText(string path, string? contents)
    {
        AppendAllText(path, contents, Utf8NoBom);
    }


    private static void AppendAllText(string path, string? contents, Encoding encoding)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));
        if (encoding == null)
            throw new ArgumentNullException(nameof(encoding));
        if (path.Length == 0)
            throw new ArgumentException(nameof(path));

        if (!string.IsNullOrEmpty(contents))
        {
            InternalWriteAllText(AsyncStreamWriter(path, encoding, true), contents);
            return;
        }

        new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read).Dispose();

    }

    private static async Task AppendAllTextAsync(string path, string? contents, Encoding encoding)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));
        if (encoding == null)
            throw new ArgumentNullException(nameof(encoding));
        if (path.Length == 0)
            throw new ArgumentException(nameof(path));

        if (!string.IsNullOrEmpty(contents))
        {
            await InternalWriteAllTextAsync(AsyncStreamWriter(path, encoding, true), contents);
            return;
        }

        await new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read).DisposeAsync();
        await Task.CompletedTask;
    }

    private static StreamWriter AsyncStreamWriter(string path, Encoding encoding, bool append) => new(new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan), encoding);


    private static async Task InternalWriteAllTextAsync(StreamWriter sw, string? contents)
    {
        using (sw)
        {
            await sw.WriteAsync(contents).ConfigureAwait(false);
            await sw.FlushAsync().ConfigureAwait(false);
        }
    }
    private static void InternalWriteAllText(StreamWriter sw, string? contents)
    {
        using (sw)
        {
            sw.Write(contents);
            sw.Flush();
        }
    }


    private static string PrivateTraceLogRequest(HttpRequestHeaders headers, HttpMethod method, Uri uri, string? content)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(General.Line());
            foreach (var item in headers)
            {
                var values = new StringBuilder();
                foreach (var it in item.Value)
                {
                    if (values.Length > 0)
                        values.Append(";");

                    values.Append($"{it}");
                }

                sb.AppendLine($"{item.Key}: {values}");
            }

            if (content != null)
            {
                sb.AppendLine(General.Line());
                var lines = content.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
                var busy = true;

                foreach (var item in lines)
                {
                    var line = item.Trim();
                    if (Check.IsEmpty(line))
                        continue;

                    if (line.StartsWith("--") || line.StartsWith("Content"))
                        busy = true;

                    if (!busy)
                        continue;

                    if (line.StartsWith("grant_type"))
                    {
                        sb.Append(General.NiceUrlParameters(item));
                        continue;
                    }

                    sb.AppendLine(line);
                    if (!line.StartsWith("Content-Disposition: attachment", StringComparison.OrdinalIgnoreCase))
                        continue;

                    sb.AppendLine();
                    sb.AppendLine("###################");
                    sb.AppendLine("#   BINARY DATA   #");
                    sb.AppendLine("###################");
                    sb.AppendLine();
                    busy = false;
                }
            }

            return $"{method}-REQUEST {uri}{sb}";
        }
        catch (Exception ex)
        {
            Exception(ex);
        }

        return string.Empty;
    }
}