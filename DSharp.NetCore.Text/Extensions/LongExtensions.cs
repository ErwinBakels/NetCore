namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class LongExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid AsGuid(this long value)
    {
        var s = $"{new string('0', 32)}{value}".Right(32);
        return s.AsGuid();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minvalue"></param>
    /// <param name="maxvalue"></param>
    /// <returns></returns>
    public static long MinMax(this long value, long minvalue, long maxvalue)
    {
        return value < minvalue ? minvalue :
            value > maxvalue ? maxvalue : value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static long Add(this long value, params object[] args)
    {
        return value + args.Sum(Safe.Long);
    }

}