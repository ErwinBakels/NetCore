namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class DoubleExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minvalue"></param>
    /// <param name="maxvalue"></param>
    /// <returns></returns>
    public static double MinMax(this double value, double minvalue, double maxvalue)
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
    public static double Add(this double value, params object[] args)
    {
        return value + args.Sum(Safe.Double);
    }
}