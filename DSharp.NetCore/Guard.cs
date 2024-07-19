// ReSharper disable UnusedMember.Global

using System.Globalization;
using System.Reflection;
using System.Text;

namespace DSharp.NetCore;

/// <summary>
/// 
/// </summary>
public sealed class Guard
{
    internal static readonly string NewLine = Environment.NewLine;
    internal static readonly CultureInfo CurrentCultureInfo = CultureInfo.CurrentCulture;
    private static Guard? _that;

    private Guard()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public static Guard That => _that ??= new Guard();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void AreEqual(object expected, object actual, string message = "")
    {
        AreEqual<object>(expected, actual, message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void AreEqual<T>(T expected, T actual, string message = "")
    {
        if (Equals(expected, actual))
            return;

        HandleExpected("Guard.AreEqual", message, expected, actual);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="ignoreCase"></param>
    /// <param name="message"></param>
    public static void AreEqual(string expected, string actual, bool ignoreCase, string message = "")
    {
        if (CompareInternal(expected, actual, ignoreCase) == 0)
            return;

        HandleExpected("Guard.AreEqual", message, expected, actual);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="delta"></param>
    /// <param name="message"></param>
    public static void AreEqual(double expected, double actual, double delta, string message = "")
    {
        if (double.IsNaN(expected) || double.IsNaN(actual) || double.IsNaN(delta))
            HandleExpected("Guard.AreEqual", message, expected.ToString(CurrentCultureInfo.NumberFormat), actual.ToString(CurrentCultureInfo.NumberFormat));

        if (Math.Abs(expected - actual) <= delta)
            return;
        HandleExpected("Guard.AreEqual", message, expected.ToString(CurrentCultureInfo.NumberFormat), actual.ToString(CurrentCultureInfo.NumberFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="delta"></param>
    /// <param name="message"></param>
    public static void AreEqual(float expected, float actual, float delta, string message = "")
    {
        if (float.IsNaN(expected) || float.IsNaN(actual) || float.IsNaN(delta))
            HandleExpected("Guard.AreEqual", message, expected.ToString(CurrentCultureInfo.NumberFormat), actual.ToString(CurrentCultureInfo.NumberFormat));

        if (Math.Abs(expected - actual) <= (double)delta)
            return;
        HandleExpected("Guard.AreEqual", message, expected.ToString(CurrentCultureInfo.NumberFormat), actual.ToString(CurrentCultureInfo.NumberFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="notExpected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void AreNotEqual<T>(T notExpected, T actual, string message = "")
    {
        if (!Equals(notExpected, actual))
            return;

        HandleNotExpected("Guard.AreNotEqual", message, notExpected, actual);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notExpected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void AreNotEqual(object notExpected, object actual, string message = "")
    {
        AreNotEqual<object>(notExpected, actual, message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notExpected"></param>
    /// <param name="actual"></param>
    /// <param name="ignoreCase"></param>
    /// <param name="message"></param>
    public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message = "")
    {
        if (CompareInternal(notExpected, actual, ignoreCase) != 0)
            return;

        HandleNotExpected("Guard.AreNotEqual", message, notExpected, actual);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notExpected"></param>
    /// <param name="actual"></param>
    /// <param name="delta"></param>
    /// <param name="message"></param>
    public static void AreNotEqual(double notExpected, double actual, double delta, string message = "")
    {
        if (Math.Abs(notExpected - actual) > delta)
            return;
        HandleNotExpected("Guard.AreNotEqual", message, notExpected.ToString(CurrentCultureInfo.NumberFormat), actual.ToString(CurrentCultureInfo.NumberFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notExpected"></param>
    /// <param name="actual"></param>
    /// <param name="delta"></param>
    /// <param name="message"></param>
    public static void AreNotEqual(float notExpected, float actual, float delta, string message = "")
    {
        if (Math.Abs(notExpected - actual) > (double)delta)
            return;
        HandleNotExpected("Guard.AreNotEqual", message, notExpected.ToString(CurrentCultureInfo.NumberFormat), actual.ToString(CurrentCultureInfo.NumberFormat));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="notExpected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void AreNotSame(object notExpected, object actual, string message = "")
    {
        if (notExpected != actual)
            return;

        HandleNotExpected("Guard.AreNotSame", message, notExpected, actual);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expected"></param>
    /// <param name="actual"></param>
    /// <param name="message"></param>
    public static void AreSame(object expected, object actual, string message = "")
    {
        if (expected == actual)
            return;

        HandleExpected("Guard.AreSame", message, expected, actual);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public static void Fail(string message = "")
    {
        HandleNotExpected("Guard.Fail", message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    public static void IsEmpty(object value, string message = "")
    {
        switch (value)
        {
            case string s when Check.IsEmpty(s):
                return;
            case string:
                HandleExpected("Guard.IsEmpty", message);
                return;
            case Guid guid when guid == Guid.Empty:
                return;
            case Uri uri when Check.IsEmpty(uri):
                return;
            case Guid:
                HandleExpected("Guard.IsEmpty", message);
                return;
            case byte[] { Length: 0 }:
                return;
            default:
                Fail($"Guard.IsEmpty: {message}");
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    public static void IsFalse(bool condition, string message = "")
    {
        if (!condition)
            return;

        HandleExpected("Guard.IsFalse", message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="expectedType"></param>
    /// <param name="message"></param>
    public static void IsInstanceOfType(object? value, Type? expectedType, string message = "")
    {
        if (expectedType != null && value == null)
            HandleExpected("Guard.IsInstanceOfType", message);

        if (value != null)
        {
            var typeInfo = value.GetType().GetTypeInfo();
            if (expectedType != null)
                if (expectedType.GetTypeInfo().IsAssignableFrom(typeInfo))
                    return;
        }

        HandleExpected("Guard.IsInstanceOfType", message, expectedType, value?.GetType());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    public static void IsNotEmpty(object value, string message = "")
    {
        switch (value)
        {
            case string s when Check.IsNotEmpty(s):
                return;
            case string:
                HandleExpected("Guard.IsNotEmpty", message);
                return;
            case Guid guid when guid != Guid.Empty:
                return;
            case Uri uri when Check.IsNotEmpty(uri):
                return;
            case Guid:
                HandleExpected("Guard.IsNotEmpty", message);
                return;
            case byte[] { Length: > 0 }:
                HandleExpected("Guard.IsNotEmpty", message);
                return;
            default:
                Fail("Guard.IsNotEmpty");
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="wrongType"></param>
    /// <param name="message"></param>
    public static void IsNotInstanceOfType(object? value, Type? wrongType, string message = "")
    {
        if (wrongType != null && value == null)
            HandleNotExpected("Guard.IsNotInstanceOfType", message);

        if (value != null)
        {
            var typeInfo = value.GetType().GetTypeInfo();
            if (wrongType != null)
                if (!wrongType.GetTypeInfo().IsAssignableFrom(typeInfo))
                    return;
        }

        HandleNotExpected("Guard.IsNotInstanceOfType", message, wrongType, value?.GetType());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    public static void IsNotNull(object? value, string message = "")
    {
        if (value != null)
            return;

        HandleExpected("Guard.IsNotNull", message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    public static void IsNull(object? value, string message = "")
    {
        if (value == null)
            return;

        HandleExpected("Guard.IsNull", message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    public static void IsTrue(bool condition, string message = "")
    {
        if (condition)
            return;

        HandleExpected("Guard.IsTrue", message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static T? ThrowsException<T>(Func<object> action, string message = "") where T : Exception => ThrowsException<T>((Action)(() => action()), message);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static T? ThrowsException<T>(Action action, string message = "") where T : Exception
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(message);

        try
        {
            action();
        }

        catch (Exception ex)
        {
            if (typeof(T) != ((object)ex).GetType())
                HandleExpected("Guard.ThrowsException", message + NewLine + ex.Message + NewLine + ex.StackTrace, typeof(T).Name, ((object)ex).GetType().Name);

            return (T)ex;
        }

        HandleExpected("Guard.ThrowsException", message, typeof(T).Name);
        return default;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<T?> ThrowsExceptionAsync<T>(Func<Task> action, string message = "") where T : Exception
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(message);

        try
        {
            await action();
        }
        catch (Exception ex)
        {
            if (typeof(T) != ((object)ex).GetType())
                HandleExpected("Guard.ThrowsException", message + NewLine + ex.Message + NewLine + ex.StackTrace, typeof(T).Name, ((object)ex).GetType().Name);

            return (T)ex;
        }
        HandleExpected("Guard.ThrowsException", message, typeof(T).Name);
        return default;
    }

    private static int CompareInternal(string expected, string actual, bool ignoreCase)
    {
        return string.Compare
            (
             expected, actual, ignoreCase
                 ? StringComparison.OrdinalIgnoreCase
                 : StringComparison.Ordinal
            );
    }

    private static void HandleExpected(string title, string message = "", object? expected = null, object? actual = null)
    {
        HandleFail(true, title, message, expected, actual);
    }

    private static void HandleFail(bool exp, string title, string message = "", object? expected = null, object? actual = null)
    {
        var not = exp ? string.Empty : "Not ";

        var sb = new StringBuilder();
        sb.AppendLine(title);

        var msg = ReplaceNulls(message);
        if (Check.IsNotEmpty(msg))
            sb.AppendLine($"Message:{msg}");

        var expectedValue = ReplaceNulls(expected);
        if (Check.IsNotEmpty(expectedValue))
            sb.AppendLine($"{not}Expected:{expectedValue}");

        var actualValue = ReplaceNulls(actual);
        if (Check.IsNotEmpty(actualValue))
            sb.AppendLine($"Actual:{actualValue}");

        throw new GuardFailedException(sb.ToString());
    }

    private static void HandleNotExpected(string title, string message = "", object? unexpected = null, object? actual = null)
    {
        HandleFail(false, title, message, unexpected, actual);
    }

    private static string ReplaceNulls(object? input)
    {
        if (input == null)
            return string.Empty;

        var input1 = input.ToString();

        return Check.IsEmpty(input1)
            ? string.Empty
            : input1.Replace("\0", "\\0");
    }

}
