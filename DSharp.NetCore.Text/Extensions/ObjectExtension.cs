namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class ObjectExtension
{
    /// <summary>
    /// Copies all filled properties to the destination
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    public static void CopyPropertiesTo(this object source, object destination)
    {
        var sourceType = source.GetType();
        var destType = destination.GetType();

        foreach (var propertyInfo in sourceType.GetProperties())
        {
            if (!propertyInfo.CanWrite)
                continue;

            var value = propertyInfo.GetValue(source, null);

            if (value == null)
                continue;

            var property = destType.GetProperty(propertyInfo.Name);

            if (property != null && property.CanWrite)
                property.SetValue(destination, value, null);
        }
    }


    /// <summary>
    /// Parse all properties to a new object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T ParseTo<T>(this object source)
    {
        var destination = Activator.CreateInstance<T>();
        if (destination == null)
            return destination;

        source.CopyPropertiesTo(destination);
        return destination;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static int CompareTo(this object source, object destination)
    {
        var sourceType = source.GetType();
        var destType = destination.GetType();

        var names = sourceType.GetProperties().Where(x => x.CanWrite).Select(x => x.Name).ToList();
        names.AddRange(destType.GetProperties().Where(x => x.CanWrite).Select(x => x.Name));

        foreach (var name in names)
        {
            var pis = sourceType.GetProperty(name);
            var pid = destType.GetProperty(name);

            if (pis is null && pid is null)
                continue;
            if (pis is null)
                return -1;
            if (pid is null)
                return 1;

            var value1 = Safe.String(pis.GetValue(source, null));
            var value2 = Safe.String(pid.GetValue(destination, null));
            var v = Check.Compare(value1, value2);
            if (v != 0)
                return v;
        }

        return 0;
    }
}
