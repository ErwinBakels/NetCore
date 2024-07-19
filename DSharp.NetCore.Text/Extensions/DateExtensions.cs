using System.Globalization;

namespace DSharp.NetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class DateExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minvalue"></param>
    /// <param name="maxvalue"></param>
    /// <returns></returns>
    public static DateTime MinMax(this DateTime value, DateTime minvalue, DateTime maxvalue)
    {
        return value < minvalue ? minvalue :
            value > maxvalue ? maxvalue : value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    // This presumes that weeks start with Monday.
    // Week 1 is the 1st week of the year with a Thursday in it.
    public static int WeekNumber(this DateTime date)
    {
        // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
        // be the same week# as whatever Thursday, Friday or Saturday are,
        // and we always get those right
        var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
        if (day is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
            date = date.AddDays(3);

        // Return the week of our adjusted day
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    /// <summary>
    /// </summary>
    /// <param name="birthday"></param>
    /// <returns>Age in years</returns>
    public static int Age(this DateTime birthday)
    {
        if (birthday > DateTime.Now || birthday == DateTime.MinValue || birthday == DateTime.MaxValue)
            return 0;

        var now = DateTime.Today;

        var age = now.Year - birthday.Year;

        if (now.Month < birthday.Month || (now.Month == birthday.Month && now.Day < birthday.Day))
            // heeft nog geen verjaardag gehad dit jaar
            age--;

        return age;
    }

}
