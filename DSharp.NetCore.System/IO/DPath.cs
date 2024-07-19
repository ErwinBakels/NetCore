
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DSharp.NetCore.IO;

/// <summary>
/// 
/// </summary>
public static class DPath
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string GetLogPath()
    {
        var list = new List<string>();
        list.AddLogs(@"C:\home");  // Azure
        list.AddLogs(@"D:\");
        list.AddLogs(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        list.AddLogs(Environment.GetFolderPath(Environment.SpecialFolder.Windows));

        var found = GetLogPath(list.ToArray());
        if (Check.IsNotEmpty(found))
            return found;

        var drives = DDrive.GetLocalDrives().OrderByDescending(x => x);
        return GetLogPath(drives.ToArray());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="locations"></param>
    /// <returns></returns>
    public static string GetLogPath(string[] locations)
    {
        foreach (var item in locations)
        {
            if (!Directory.Exists(item))
                continue;

            var filename = Path.Combine(item, $"{Guid.NewGuid()}.tmp");
            try
            {
                File.WriteAllText(filename, filename);
                File.Delete(filename);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Could not write {filename}. {ex.Message}");
                continue;
            }
            return item;
        }

        return string.Empty;
    }

    private static void AddLogs(this ICollection<string> list, string location)
    {
        list.Add(Path.Combine(location, "Logs"));
        list.Add(Path.Combine(location, "Log"));
        list.Add(Path.Combine(location, "LogFiles"));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="destinationPath"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static string GoBackToPath(string path, string destinationPath)
    {
        Guard.IsNotEmpty(path);
        Guard.IsNotEmpty(destinationPath);

        var currentFolder = Path.GetFileName(path);

        if (Check.CompareCi(destinationPath, currentFolder) == 0)
            return path;

        while (true)
        {
            currentFolder = Path.GetFileName(path);

            if (Check.CompareCi(destinationPath, currentFolder) == 0)
                return path;

            var parentPath = Directory.GetParent(path);
            if (parentPath == null)
                break;

            var newPath = parentPath.FullName;

            if (Check.CompareCi(newPath, path) == 0)  // reached the root
                break;

            path = newPath;

        }

        throw new DirectoryNotFoundException(destinationPath);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static string GoBackToFile(string path, string searchPattern)
    {
        Guard.IsNotEmpty(path);
        Guard.IsNotEmpty(searchPattern);

        while (true)
        {
            var files = Directory.EnumerateFiles(path, searchPattern);
            if (files.Any())
                return path;

            var parentPath = Directory.GetParent(path);
            if (parentPath == null)
                break;

            var newPath = parentPath.FullName;
            if (Check.CompareCi(newPath, path) == 0) // reached the root
                break;

            path = newPath;
        }

        throw new FileNotFoundException(searchPattern);
    }

    private static readonly Dictionary<KnownFolder, Guid> Guids = new()
    {
        [KnownFolder.Contacts] = new("56784854-C6CB-462B-8169-88E350ACB882"),
        [KnownFolder.Downloads] = new("374DE290-123F-4565-9164-39C4925E467B"),
        [KnownFolder.Favorites] = new("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
        [KnownFolder.Links] = new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
        [KnownFolder.SavedGames] = new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
        [KnownFolder.SavedSearches] = new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="knownFolder"></param>
    /// <returns></returns>
    public static string GetPath(KnownFolder knownFolder)
    {
        return SHGetKnownFolderPath(Guids[knownFolder], 0);
    }

    [DllImport("shell32", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
    internal static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, nint hToken = 0);
}
