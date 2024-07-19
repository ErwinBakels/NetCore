namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string NameWithGenerics(this Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (type.IsArray)
            return $"{type.GetElementType()?.Name}[]";

        if (!type.IsGenericType)
            return type.Name;

        var name = type.GetGenericTypeDefinition().Name;
        var index = name.IndexOf('`');
        var newName = index == -1 ? name : name.Substring(0, index);

        var list = type.GetGenericArguments().Select(NameWithGenerics).ToList();
        return $"{newName}<{string.Join(",", list)}>";
    }
}