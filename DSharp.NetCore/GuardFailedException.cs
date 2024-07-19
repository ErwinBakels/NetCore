namespace DSharp.NetCore;

/// <summary>
/// 
/// </summary>
public class GuardFailedException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    public GuardFailedException(string msg)
        : base(msg)
    {
    }
}