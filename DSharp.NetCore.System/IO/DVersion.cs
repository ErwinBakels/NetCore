namespace DSharp.NetCore.IO;

/// <summary>
/// 
/// </summary>
public class DVersion
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    /// <returns></returns>
    public static DVersion Parse(Version? version)
    {
        if (version == null)
            return new DVersion();

        return new DVersion
        {
            Major = version.Major,
            Minor = version.Minor,
            Build = version.Build
        };
    }

    /// <summary>
    /// 
    /// </summary>
    public int Major { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Minor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Build { get; set; }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Build}";
    }
}