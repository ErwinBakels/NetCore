using System.Reflection;

namespace DSharp.NetCore.IO;

/// <summary>
/// 
/// </summary>
public class DApplicationInfo : DAssembly
{
    /// <summary>
    /// 
    /// </summary>
    public List<DAssembly> References { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static DApplicationInfo Get(Type type)
    {
        var result = new DApplicationInfo();

        var assembly = Assembly.GetAssembly(type);
        if (assembly == null)
            return result;

        var name = assembly.GetName();

        result.Name = $"{name.Name}";
        result.Version = DVersion.Parse(name.Version);

        foreach (var item in assembly.GetReferencedAssemblies())
        {
            var a = Parse(item);
            result.References.Add(a);
        }
        return result;
    }

}