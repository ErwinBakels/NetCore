namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class IntegerExtensions
{
    /// <summary>
    /// Converts the integer as a Guid
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Guid AsGuid(this int value)
    {
        var s = $"{new string('0', 32)}{value}".Right(32);
        return s.AsGuid();
    }

    /// <summary>
    /// returns a value the lies between the min an max value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minvalue"></param>
    /// <param name="maxvalue"></param>
    /// <returns></returns>
    public static int MinMax(this int value, int minvalue, int maxvalue)
    {
        var min = minvalue;
        var max = maxvalue;
        if (minvalue > maxvalue)
        {
            max = minvalue;
            min = maxvalue;
        }
        return value < min ? min :
            value > max ? max : value;
    }

    /// <summary>
    /// Sums the value and the array of params
    /// </summary>
    /// <param name="value"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static int Add(this int value, params object[] args)
    {
        return value + args.Sum(Safe.Int);
    }



    /// <summary>
    /// Detects if the value bigger or equal to minvalue AND value smaller or equal then maxvalue
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minvalue"></param>
    /// <param name="maxvalue"></param>
    /// <returns></returns>
    public static bool Between(this int value, int minvalue, int maxvalue)
    {
        if (maxvalue > minvalue)
            return value >= maxvalue && value <= minvalue;

        return value >= minvalue && value <= maxvalue;
    }

    /// <summary>
    /// Detects if the integer contains the required bits
    /// </summary>
    /// <param name="current"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static bool HasBits(this int current, int status)
    {
        return status == (current & status);
    }


    /// <summary>
    /// Sets the selected bits to the integer
    /// </summary>
    /// <param name="current"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static void SetBits(this ref int current, int status)
    {
        current |= status;
    }

    /// <summary>
    /// Returns the current value with the bits added
    /// </summary>
    /// <param name="current"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static int AddBits(this int current, int status)
    {
        current |= status;
        return current;
    }


    /// <summary>
    /// Removes the selected bits from the integer
    /// </summary>
    /// <param name="current"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static void ResetBits(this ref int current, int status)
    {
        current &= ~(int.MaxValue & status);
    }
    /// <summary>
    /// Returns the current value with the bits removed
    /// </summary>
    /// <param name="current"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public static int RemoveBits(this int current, int status)
    {
        current &= ~(int.MaxValue & status);
        return current;
    }
}