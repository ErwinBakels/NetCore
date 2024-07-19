using System.Collections;
using System.Reflection;
using DSharp.NetCore.Attributes;

namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnumType"></typeparam>
    /// <returns></returns>
    public static IEnumerable<TEnumType> EnumerateValues<TEnumType>() where TEnumType : Enum
    {
        var enumTypeObject = typeof(TEnumType);
        IEnumerable values = Enum.GetValues(enumTypeObject);
        return values.Cast<TEnumType>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string? GetStringValue(this Enum enumValue)
    {
        return (enumValue.GetStringValuesWithPreferences() ?? Array.Empty<StringValueAttribute>())
                        .Select(attribute => attribute.StringValue)
                        .FirstOrDefault();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetAllStringValues(this Enum value)
    {
        var valuesPreferencePairs = value.GetStringValuesWithPreferences();
        return (valuesPreferencePairs ?? Array.Empty<StringValueAttribute>()).Select(pair => pair.StringValue);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnumType"></typeparam>
    /// <param name="stringValue"></param>
    /// <returns></returns>
    /// <exception cref="UnmatchedStringValueException"></exception>
    public static TEnumType? ParseToEnum<TEnumType>(this string stringValue) where TEnumType : Enum
    {
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        if (TryParseStringValueToEnum(stringValue, out TEnumType? lRet))
            return lRet;

        throw new UnmatchedStringValueException(stringValue, typeof(TEnumType));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnumType"></typeparam>
    /// <param name="stringValueCollection"></param>
    /// <returns></returns>
    public static List<TEnumType?> ParseToEnumList<TEnumType>(this IEnumerable<string> stringValueCollection) where TEnumType : Enum
    {
        return stringValueCollection.Select(ParseToEnum<TEnumType>).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnumType"></typeparam>
    /// <param name="stringValue"></param>
    /// <param name="parsedValue"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParseStringValueToEnum<TEnumType>(this string stringValue, out TEnumType? parsedValue) where TEnumType : Enum
    {
        if (stringValue == null)
            throw new ArgumentNullException(nameof(stringValue), "Input string may not be null.");

        var lowerStringValue = stringValue.ToLower();

        foreach (var enumValue in EnumerateValues<TEnumType>())
        {
            var enumStrings = enumValue.GetAllStringValues().Select(text => text.ToLower());

            if (enumStrings.Contains(lowerStringValue))
            {
                parsedValue = enumValue;
                return true;
            }
        }

        parsedValue = default;
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool HasAttribute(this PropertyInfo info, string name)
    {
        return info.CustomAttributes.Any(attr => attr.AttributeType.Name == name);
    }

    private static IEnumerable<StringValueAttribute>? GetStringValuesWithPreferences(this Enum enumValue)
    {
        var stringValueAttributes = enumValue.GetType()
                                             .GetField(enumValue.ToString())?
                                             .GetCustomAttributes(typeof(StringValueAttribute), false)
                                             .Cast<StringValueAttribute>()
                                             .ToList();

        if (stringValueAttributes != null && !stringValueAttributes.Any())
            stringValueAttributes.Add(new StringValueAttribute(enumValue.ToString()));

        return stringValueAttributes;
    }


}
