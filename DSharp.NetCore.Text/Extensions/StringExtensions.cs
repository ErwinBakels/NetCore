using System.Collections;
using System.Text;

namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class StringExtensions
{

    internal static Guid AsGuid(this string value)
    {
        //00000000-0000-0000-0000-000000000000
        return new Guid($"{value[..8]}-{value.Substring(8, 4)}-{value.Substring(12, 4)}-{value.Substring(16, 4)}-{value.Substring(20, 12)}");
    }


    /// <summary>
    /// Checks if the first value is filled, if not, it takes the second value
    /// </summary>
    /// <returns></returns>
    public static string Else(this string value, params string[] values)
    {
        if (!string.IsNullOrEmpty(value))
            return value;

        foreach (var v in values)
        {
            if (!string.IsNullOrEmpty(v))
                return v;
        }

        return string.Empty;
    }

    /// <summary>
    /// Returns the left part of the current string and fills the right side with the filler char
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="filler"></param>
    /// <returns></returns>
    public static string Left(this string value, int length, char filler)
    {
        var left = Left(value, length);
        if (left.Length == length)
            return left;

        return $"{left}{new string(filler, length - left.Length)}";
    }

    /// <summary>
    /// Returns the left part of the current string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Left(this string value, int length)
    {

        if (value.Length == 0)
            return "";

        return length >= value.Length
            ? value
            : value[..length];
    }

    /// <summary>
    /// Returns the right part of the current string and fills the left side with the filler char
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <param name="filler"></param>
    /// <returns></returns>
    public static string Right(this string value, int length, char filler)
    {
        var right = Right(value, length);
        if (right.Length == length)
            return right;

        return $"{new string(filler, length - right.Length)}{right}";
    }

    /// <summary>
    /// Returns the right part of the current string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Right(this string value, int length)
    {
        if (value.Length == 0)
            return "";

        var strlength = value.Length;

        return length >= strlength
            ? value
            : value.Substring(strlength - length, length);
    }

    /// <summary>
    /// Corrects text with accents on it ä becomes a
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string TextCorrection(this string value)
    {

        const string oldchars = "¢£¥ª²³µ¹ºÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþ";
        const string newchars = "cFYa23u1oAAAAAAACEEEEIIIIDNOOOOOx0UUUUYpBaaaaaaaceeeeiiiionooooo0uuuuyp";

        var sb = new StringBuilder();

        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i].ToString();

            if (oldchars.Contains(c))
                sb.Append(newchars[i]);
            else
                sb.Append(c);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="safechars"></param>
    /// <returns></returns>
    public static string OnlyChars(this string value, string safechars)
    {

        var sb = new StringBuilder();

        foreach (var c in value)
        {
            if (safechars.Contains(c))
                sb.Append(c);
        }

        return sb.ToString();
    }



    /// <summary>
    /// only returns 0123456789,.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string OnlyNumeric(this string value)
    {
        return value.OnlyChars("0123456789,.");
    }

    /// <summary>
    ///  only returns ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string OnlyAtoZ(this string value)
    {
        return value.OnlyChars("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
    }

    /// <summary>
    /// only returns ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz_-0123456789
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string OnlyAscii(this string value)
    {
        return value.OnlyChars("ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz_-0123456789");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string RemoveTrailingChars(this string value, string chars)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        while (true)
        {
            if (value.Length == 0)
                break;

            var last = value.Right(1);
            if (!chars.Contains(last))
                break;

            value = value.Left(value.Length - 1);
        }

        return value;
    }



    /// <summary>
    /// Add a NewLine to the string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string AddNewLine(this string value)
    {
        return $"{value}{Environment.NewLine}";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value">TEXT</param>
    /// <returns>'TEXT'</returns>
    public static string Quoted(this string value)
    {
        return Embrace(value, "'");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value">TEXT</param>
    /// <returns>"TEXT"</returns>
    public static string DQuoted(this string value)
    {
        return Embrace(value, '"');
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value">TEXT</param>
    /// <returns>&lt;TEXT&gt;</returns>
    public static string Embrace(this string value)
    {
        return Embrace(value, '<');
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value">TEXT</param>
    /// <param name="code">( { [ &lt;  or your own char</param>
    /// <returns>(TEXT) {TEXT} [TEXT] &lt;TEXT&gt;     yourowncharTEXTyourownchar</returns>
    public static string Embrace(this string value, char code)
    {
        Guard.IsNotNull(code);

        var (code2, _) = GetCode2(code);

        return $"{code}{value}{code2}";
    }

    private static (char, bool) GetCode2(char code)
    {
        var code2 = code switch
        {
            '(' => ')',
            '{' => '}',
            '[' => ']',
            '<' => '>',
            _ => code
        };

        var ok = !Check.IsAtoZ(code2.ToString());
        return (code2, ok);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="value">TEXT</param>
    /// <returns>Text without the braces</returns>
    public static string Unbrace(this string value)
    {
        if (Check.IsEmpty(value))
            return value;

        var code = value[0];

        var (code2, ok) = GetCode2(code);
        if (!ok)
            return value;

        if (!value.StartsWith(code.ToString()))
            return value;

        var newValue = value[1..];
        return newValue.EndsWith(code2.ToString())
            ? newValue[..^1]
            : value;
    }



    /// <summary>
    /// Embraces the current string with the provided text
    /// </summary>
    /// <param name="value"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Embrace(this string value, string text)
    {
        Guard.IsNotNull(text);
        return $"{text}{value}{text}";
    }

    /// <summary>
    /// Creates a tag arround the value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public static string Tag(this string value, string property)
    {
        Guard.IsNotNull(property);
        return $"<{property}>{value}</{property}>";
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="dic"></param>
    /// <param name="start">#{</param>
    /// <param name="end">}#</param>
    /// <returns></returns>
    public static string ReplaceTokens(this string content, IDictionary dic, string start = "#{", string end = "}#")
    {

        if (dic.Count == 0 || content.Length < 2)
            return content;

        var token = GetFirstToken(content, 0, start, end);
        if (string.IsNullOrWhiteSpace(token))
            return content;


        var fileType = content[0] switch
        {
            '{' => "json",
            '[' => "json",
            '<' => "xml",
            _ => ""
        };

        while (!string.IsNullOrWhiteSpace(token))
        {
            var value = string.Empty;

            var keys = dic.Keys;

            foreach (string key in keys)
            {
                if (!Check.AreEqualCi(key, token))
                    continue;

                value = $"{dic[key]}";

                switch (fileType)
                {
                    case "json":
                        value = value.EscapeJson();
                        break;
                    case "xml":
                        value = value.EscapeXml();
                        break;
                }

                break;
            }

            content = content.ReplaceToken(token, value, start, end);
            token = GetFirstToken(content, 0, start, end);
        }

        return content;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="startPosition"></param>
    /// <param name="start">#{</param>
    /// <param name="end">}#</param>
    /// <returns></returns>
    public static string GetFirstToken(this string content, int startPosition = 0, string start = "#{", string end = "}#")
    {
        var p1 = content.IndexOf(start, startPosition, StringComparison.OrdinalIgnoreCase);
        if (p1 == -1)
            return string.Empty;
        var p2 = content.IndexOf(end, p1 + start.Length, StringComparison.OrdinalIgnoreCase);
        if (p2 == -1)
            return string.Empty;

        return content.Substring(p1 + start.Length, p2 - p1 - start.Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="token"></param>
    /// <param name="value"></param>
    /// <param name="start">#{</param>
    /// <param name="end">}#</param>
    /// <returns></returns>
    public static string ReplaceToken(this string content, string token, string value = "", string start = "#{", string end = "}#")
    {
        var oldValue = $"{start}{token}{end}";
        return content.Replace(oldValue, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string EscapeJson(this string value)
    {
        var bs = @"\";
        var qt = '"';
        StringBuilder sb = new StringBuilder();
        foreach (char c in value)
            switch (c)
            {
                case '"':
                    sb.Append($"{bs}{qt}");
                    break;
                case '\\':
                    sb.Append($"{bs}{bs}");
                    break;
                case '\b':
                    sb.Append($"{bs}b");
                    break;
                case '\f':
                    sb.Append($"{bs}f");
                    break;
                case '\n':
                    sb.Append($"{bs}n");
                    break;
                case '\r':
                    sb.Append($"{bs}r");
                    break;
                case '\t':
                    sb.Append($"{bs}t");
                    break;
                default:
                    if (c < 32)
                        sb.AppendFormat(@"\u{0:X4}", (int)c);
                    else
                        sb.Append(c);
                    break;
            }

        return sb.ToString();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string EscapeXml(this string value)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in value)
            switch (c)
            {
                case '"':
                    sb.Append("&quot;");
                    break;
                case '\'':
                    sb.Append("&apos;");
                    break;
                case '<':
                    sb.Append("&lt;");
                    break;
                case '>':
                    sb.Append("&gt;");
                    break;
                case '&':
                    sb.Append("&amp;");
                    break;
                default:
                    if (c < 32)
                        sb.Append($"&#{(int)c};");
                    else
                        sb.Append(c);
                    break;
            }

        return sb.ToString();
    }
}
