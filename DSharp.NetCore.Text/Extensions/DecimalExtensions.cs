namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class DecimalExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minvalue"></param>
    /// <param name="maxvalue"></param>
    /// <returns></returns>
    public static decimal MinMax(this decimal value, decimal minvalue, decimal maxvalue)
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
    public static decimal Add(this decimal value, params object[] args)
    {
        return value + args.Sum(Safe.Decimal);
    }

}
