using System.Reflection;

namespace DSharp.NetCore.IO;

/// <summary>
/// 
/// </summary>
public class DAssembly
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public DVersion Version { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static DAssembly Parse(AssemblyName name)
    {
        return new DAssembly
        {
            Name = $"{name.Name}",
            Version = DVersion.Parse(name.Version)
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Name} v{Version}";
    }
}