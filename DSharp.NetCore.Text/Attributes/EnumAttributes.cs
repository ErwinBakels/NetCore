namespace DSharp.NetCore.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class StringValueAttribute : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public StringValueAttribute(string value)
    {
        StringValue = value;
    }

    internal string StringValue { get; }
}