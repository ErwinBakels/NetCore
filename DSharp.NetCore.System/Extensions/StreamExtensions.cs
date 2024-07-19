namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static string ToString(this Stream stream)
    {
        using var rd = new StreamReader(stream);
        return rd.ReadToEnd();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="text"></param>
    public static void FromString(this Stream stream, string text)
    {
        using var sw = new StreamWriter(stream);
        sw.Write(text);
        sw.Flush();
        sw.Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static byte[] ToArray(this Stream stream)
    {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="bytes"></param>
    public static void FromArray(this Stream stream, byte[] bytes)
    {
        //Copy the attachment stream to the stream
        new MemoryStream(bytes).CopyTo(stream);
    }
}