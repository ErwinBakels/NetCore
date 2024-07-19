namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// Takes the first byte array, If that array is NULL or length is 0; It takes the second Byte Array.
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns>Byte Array</returns>
    public static byte[]? Else(this byte[]? value1, byte[]? value2)
    {
        return value1 is { Length: > 0 } ? value1 : value2;
    }
}