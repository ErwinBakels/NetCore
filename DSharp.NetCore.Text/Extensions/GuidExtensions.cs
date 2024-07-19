namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class GuidExtensions
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int AsInteger(this Guid value)
    {
        var s = Safe.String(value).Replace("-", "");
        return Safe.Int(s);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long AsLong(this Guid value)
    {
        var s = Safe.String(value).Replace("-", "");
        return Safe.Long(s);
    }

}
