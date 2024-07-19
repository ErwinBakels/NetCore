using System.Diagnostics;

using DSharp.NetCore.Extensions;

namespace DSharp.NetCore.IO;

/// <summary>
/// 
/// </summary>
public static class DDirectory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="includingSubdirectories"></param>
    /// <returns></returns>
    public static bool CleanUp(string path, bool includingSubdirectories = false)
    {
        Guard.IsNotEmpty(path);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return true;
        }

        foreach (var item in Directory.GetFiles(path))
        {
            try
            {
                var filename = Path.Combine(path, item);
                File.SetAttributes(filename, FileAttributes.Normal);
                File.Delete(filename);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CleanUpDirectory() Exception: {ex.Message}");
                return false;
            }
        }

        if (!includingSubdirectories)
            return true;

        foreach (var directory in Directory.GetDirectories(path))
        {
            try
            {
                Directory.Delete(directory, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CleanUpDirectory() Exception: {ex.Message}");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="option"></param>
    /// <param name="searchPatterns"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetFiles(string folder, SearchOption option, params string[] searchPatterns)
    {
        var files = new List<string>();
        foreach (var searchPattern in searchPatterns)
        {
            files.AddRange(Directory.GetFiles(folder, searchPattern, option));
        }

        return files.Distinct();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="searchPatterns"></param>
    public static void ReplaceTokens(string folder, params string[] searchPatterns)
    {
        ReplaceTokens(folder, SearchOption.TopDirectoryOnly, searchPatterns);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="option"></param>
    /// <param name="searchPatterns"></param>
    public static void ReplaceTokens(string folder, SearchOption option, params string[] searchPatterns)
    {
        var files = GetFiles(folder, option, searchPatterns).ToList();
        if (files.Count == 0)
            return;

        var env = Environment.GetEnvironmentVariables();

        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var newContent = content.ReplaceTokens(env);
            if (content.GetHashCode() == newContent.GetHashCode())
                continue;

            File.WriteAllText(file, newContent);
        }
    }
}