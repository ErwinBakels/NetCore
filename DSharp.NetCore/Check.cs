using System.Diagnostics.CodeAnalysis;

namespace DSharp.NetCore;

/// <summary>
/// 
/// </summary>
public static class Check
{
    /// <summary>
    /// If all of the objects are empty, then true
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static bool AreEmpty(params object?[] objects) => objects.All(IsEmpty);

    /// <summary>
    /// Compares the two strings on Ordinal way
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool AreEqual(string value1, string value2) => Compare(value1, value2) == 0;

    /// <summary>
    /// Compares the two strings on Ordinal way but Case Insensitive
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool AreEqualCi(string value1, string value2) => CompareCi(value1, value2) == 0;

    /// <summary>
    /// If all of the objects are not empty, then true
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static bool AreNotEmpty(params object?[] objects) => objects.All(IsNotEmpty);

    /// <summary>
    /// Compares the two strings on Ordinal way
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool AreNotEqual(string value1, string value2) => Compare(value1, value2) != 0;

    /// <summary>
    /// Compares the two strings on Ordinal way but Case Insensitive
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool AreNotEqualCi(string value1, string value2) => CompareCi(value1, value2) != 0;

    /// <summary>
    /// Compare 2 safe strings
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static int Compare(string value1, string value2)
    {
        return string.Compare(value1, value2, StringComparison.Ordinal);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static int CompareCi(string value1, string value2)
    {
        return string.Compare(value1, value2, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// if Source contains 'value' then true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Contains(string source, string value)
    {
        if (IsEmpty(source))
            return false;

        return IsEmpty(value) || source.Contains(value);
    }

    /// <summary>
    /// if Source contains 'value' then true
    /// </summary>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ContainsCi(string source, string value)
    {
        if (IsEmpty(source))
            return false;

        return IsEmpty(value) || source.Contains(value, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// If one of the sources contains 'value' then true
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sources"></param>
    /// <returns></returns>
    public static bool ContainingIn(string value, params string[] sources)
    {
        return IsEmpty(value) || sources.Any(item => Contains(item, value));
    }

    /// <summary>
    /// If one of the sources contains 'value' then true
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sources"></param>
    /// <returns></returns>

    public static bool ContainingInCi(string value, params string[] sources)
    {
        return IsEmpty(value) || sources.Any(item => ContainsCi(item, value));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="check"></param>
    /// <returns></returns>
    public static bool HasBit(int source, int check)
    {
        return (source & check) != 0;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="check"></param>
    /// <param name="resultTrue"></param>
    /// <param name="resultFalse"></param>
    /// <returns></returns>
    public static string HasBit(int source, int check, string resultTrue, string resultFalse = "")
    {
        return HasBit(source, check) ? resultTrue : resultFalse;
    }

    /// <summary>
    /// If the object is Null or Empty string, then true
    /// </summary>
    /// <returns></returns>
    public static bool IsEmpty([NotNullWhen(false)] object? obj)
    {
        return obj switch
        {
            null => true,
            string => obj.Equals(string.Empty),
            Guid => obj.Equals(Guid.Empty),
            _ => false
        };
    }

    /// <summary>
    /// If one of the objects is empty, then true
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static bool IsEmpty(params object?[] objects) => objects.Any(IsEmpty);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsGuid(string value)
    {
        if (IsEmpty(value))
            return false;

        try
        {
            _ = new Guid(value);
            return true;
        }
        catch
        {
            // do nothing
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsAtoZ(string value)
    {
        if (IsEmpty(value))
            return false;

        const string ascii = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return ascii.Contains(value);
    }


    /// <summary>
    /// If the object is not Null and is not Empty string, then true
    /// </summary>
    /// <returns></returns>
    public static bool IsNotEmpty(object? obj) => !IsEmpty(obj);

    /// <summary>
    /// If one of the objects is not empty, then true
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static bool IsNotEmpty(params object?[] objects) => objects.Any(IsNotEmpty);

    /// <summary>
    /// Checks if the string only contains the allowed characters
    /// </summary>
    /// <param name="value"></param>
    /// <param name="allowed"></param>
    /// <returns></returns>
    public static bool HasOnlyCharactersIn(this string value, string allowed)
    {
        return value.All(allowed.Contains);
    }
}
