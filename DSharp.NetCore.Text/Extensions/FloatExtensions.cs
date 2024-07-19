namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class FloatExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minvalue"></param>
    /// <param name="maxvalue"></param>
    /// <returns></returns>
    public static float MinMax(this float value, float minvalue, float maxvalue)
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
    public static float Add(this float value, params object[] args)
    {
        return value + args.Sum(Safe.Float);
    }
}