namespace DSharp.NetCore.Attributes;

/// <summary>
/// 
/// </summary>
public class UnmatchedStringValueException : Exception
{
    /// <inheritdoc />
    public UnmatchedStringValueException(string value, Type type)
        : base($"String does not match to any value of the specified Enum. Attempted to Parse {value} into an Enum of type {type.Name}.")
    {
    }
}