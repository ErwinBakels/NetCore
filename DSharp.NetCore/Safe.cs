using System.Globalization;
using System.Text;

namespace DSharp.NetCore;

/// <summary>
/// 
/// </summary>
public static class Safe
{
    /// <summary>
    /// Get the safe gender as an int
    /// </summary>
    /// <param name="value"></param>
    /// <returns>0 = unknown, 1 = Male, 2 = Female</returns>
    public static int Gender(string value)
    {
        return value.ToLower() switch
        {
            "1" => 1,
            "m" => 1,
            "h" => 1,
            "u" => 1,
            "2" => 2,
            "f" => 2,
            "v" => 2,
            "d" => 2,
            _ => 0
        };
    }

    /// <summary>
    /// Get the safe gender as an string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="language"></param>
    /// <returns>0 = U, 1 = M, 2 = F (Depending on language EN,NL,DE,FR,IT)</returns>
    public static string Gender(int value, string language = "EN")
    {
        return $"{GenderLong(value, language)[0]}";
    }
    /// <summary>
    /// Get the safe gender as an string
    /// </summary>
    /// <param name="value"></param>
    /// <param name="language"></param>
    /// <returns>0 = unknown, 1 = Male, 2 = Female (Depending on language EN,NL,DE,FR,IT)</returns>
    public static string GenderLong(int value, string language = "EN")
    {
        var dic = new Dictionary<string, string>
        {
            { "EN", "Unknown;Male;Female" },
            { "NL", "Onbekend;Man;Vrouw" },
            { "DE", "Unbekannt;Mann;Frau" },
            { "FR", "Inconnu;Homme;Femme" },
            { "IT", "Sconosciuto;Uomo;Donna" },
        };

        var item = dic[language.ToUpper()];
        if (string.IsNullOrEmpty(item))
            item = dic["EN"];

        var names = item.Split(';');
        return names[value.MinMax(0, 2)];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool Boolean(object? obj)
    {
        if (obj == null)
            return false;

        return String(obj).ToLower() switch
        {
            "1" => true,
            "y" => true,
            "yes" => true,
            "w" => true,
            "waar" => true,
            "j" => true,
            "ja" => true,
            "t" => true,
            "true" => true,
            _ => false
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static byte[] ByteArray(object? obj)
    {
        if (obj == null)
            return [];

        if (obj.GetType() == typeof(byte[]))
            return (byte[])obj;

        return [];
    }


    private static byte[] DaysInMonth365 => [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    private static byte[] DaysInMonth366 => [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];


    private static int DaysInMonth(int year, int month)
    {
        if (month < 1 || month > 12)
            return 0;
        return (System.DateTime.IsLeapYear(year) ? DaysInMonth366 : DaysInMonth365)[month - 1];
    }



    private static bool IsDateValid(int year, int month, int day)
    {
        return day <= DaysInMonth(year, month);
    }

    /// <summary>
    /// Returns the first valid date
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime NearestValidDate(string value)
    {
        var year = 1800;
        var month = 1;
        var day = 1;


        if (value.Contains("/"))
        {
            // usa
            var s = value.Split('/');
            if (s.Length != 3)
                return System.DateTime.MinValue;

            var v1 = Int(s[0]);
            var v2 = Int(s[1]);
            var v3 = Int(s[2]);

            if (v1 > 100)
            {
                year = v1;
                month = v2;
                day = v3;
            }
            else
            {
                month = v1;
                day = v2;
                year = v3;
            }
        }
        else if (value.Contains("-"))
        {
            var s = value.Split('-');
            if (s.Length != 3)
                return System.DateTime.MinValue;

            var v1 = Int(s[0]);
            var v2 = Int(s[1]);
            var v3 = Int(s[2]);

            if (v1 > 100)
            {
                year = v1;
                month = v2;
                day = v3;
            }
            else
            {
                day = v1;
                month = v2;
                year = v3;
            }
        }


        year = year.MinMax(1770, 9999);
        month = month.MinMax(1, 12);
        day = day.MinMax(1, 31);

        var valid = IsDateValid(year, month, day);
        while (!valid)
        {
            day--;
            valid = IsDateValid(year, month, day);
        }

        return new DateTime(year, month, day);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime DateTime(object? value)
    {
        if (value is DateTime time)
            return time;

        if (System.DateTime.TryParse($"{value}", out var d))
            return d;

        return System.DateTime.MinValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static DateTime DatePicker(DateTime? value)
    {
        var result = new DateTime(1900, 1, 1);
        return !value.HasValue ? result : value.Value < result ? result : value.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int Int(object? obj)
    {
        return int.Parse(Number(obj), NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static double Double(object? obj)
    {
        return double.Parse(Number(obj), NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static float Float(object? obj)
    {
        return float.Parse(Number(obj), NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static long Long(object? obj)
    {
        return long.Parse(Number(obj), NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static decimal Decimal(object? obj)
    {
        return decimal.Parse(Number(obj), NumberStyles.Any, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string String(object? obj)
    {
        return obj switch
        {
            null => string.Empty,
            DateTime dateTime => dateTime.ToString("s"),
            _ => $"{obj}"
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string Number(object? obj)
    {
        if (obj == null)
            return "0";

        var negative = false;

        var s = String(obj);

        if (string.IsNullOrWhiteSpace(s))
            return "0";

        if (s.StartsWith("-"))
            negative = true;

        var comma = s.LastIndexOf(',');
        var punt = s.LastIndexOf('.');

        if (comma > punt)
        {
            s = s.Replace(".", "");
            s = s.Replace(",", ".");
        }
        else
            s = s.Replace(",", "");

        s = s.OnlyNumeric();
        var ix1 = s.IndexOf('.');

        if (ix1 > 0 && s.Length > ix1)
        {
            var ix2 = s.IndexOf('.', ix1 + 1);
            if (ix2 > 0)
                s = s.Substring(0, ix2);
        }

        s = s.TrimStart('0');

        if (s.Contains("."))
            s = s.TrimEnd('0');

        s = s.TrimEnd('.');
        if (s.Length == 0 || s.StartsWith("."))
            s = "0" + s;

        if (negative)
            s = "-" + s;

        return s;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Guid Guid(object? obj)
    {
        if (obj == null)
            return System.Guid.Empty;

        try
        {
            return obj is Guid guid
                ? guid
                : new Guid($"{obj}");
        }
        catch
        {
            // do nothing
        }
        return System.Guid.Empty;
    }

    /// <summary>
    /// Resturns the data as a safe string, if the data is longer than 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static string Data(object? obj, int max = 3000)
    {
        if (obj == null)
            return string.Empty;

        var value = $"{obj}";

        if (value.Length > max)
            return $"{Environment.NewLine}Large Data: {value.Length} bytes";

        return value.Length == 0
            ? string.Empty
            : $"{Environment.NewLine}{value}";
    }



    private static int MinMax(this int value, int minvalue, int maxvalue)
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

    private static string OnlyNumeric(this string value)
    {
        return value.OnlyChars("0123456789,.");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="safechars"></param>
    /// <returns></returns>
    private static string OnlyChars(this string value, string safechars)
    {
        var sb = new StringBuilder();
        foreach (char c in value.Where(safechars.Contains))
            sb.Append(c);

        return sb.ToString();
    }
}
