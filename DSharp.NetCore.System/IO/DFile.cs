namespace DSharp.NetCore.IO;

/// <summary>
/// 
/// </summary>
public static class DFile
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="memoryStream"></param>
    public static void WriteMemoryStream(string filename, MemoryStream? memoryStream)
    {
        if (string.IsNullOrWhiteSpace(filename))
            throw new ArgumentNullException(nameof(filename));

        if (memoryStream == null)
            throw new ArgumentNullException(nameof(memoryStream));


        var bytes = new byte[memoryStream.Length];

        File.WriteAllBytes(filename, bytes);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static MemoryStream? ToMemoryStream(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            throw new ArgumentNullException(nameof(filename));

        if (!File.Exists(filename))
            return default;

        var bytes = File.ReadAllBytes(filename);
        return new MemoryStream(bytes);
    }
}