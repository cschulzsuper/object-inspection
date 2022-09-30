using System;
using System.Globalization;

namespace ChristianSchulz.ObjectInspection.Shared;

public struct DateTimeNumbers
{
    public DateTimeNumbers(DateTime globalDateTime)
    {
        var dateOnly = DateOnly.FromDateTime(globalDateTime);
        var timeSpan = globalDateTime.TimeOfDay;

        Date = dateOnly.DayNumber;
        Time = (int)Math.Round(timeSpan.TotalMilliseconds);
    }

    public DateTimeNumbers(int date, int time)
    {
        Date = date;
        Time = time;
    }

    public int Date { get; init; }
    public int Time { get; init; }

    public void Deconstruct(out int date, out int time)
    {
        date = Date;
        time = Time;
    }

    public DateTime ToGlobalDateTime()
    {
        var dateOnly = DateOnly.FromDayNumber(Date);
        var timeSpan = TimeSpan.FromMilliseconds(Time);
        var timeOnly = TimeOnly.FromTimeSpan(timeSpan);

        return dateOnly.ToDateTime(timeOnly, DateTimeKind.Utc);
    }

    public DateTime ToLocalDateTime()
    {
        var dateOnly = DateOnly.FromDayNumber(Date);
        var timeSpan = TimeSpan.FromMilliseconds(Time);
        var timeOnly = TimeOnly.FromTimeSpan(timeSpan);

        return dateOnly.ToDateTime(timeOnly, DateTimeKind.Utc).ToLocalTime();
    }

    public string ToLocalDateOnlyString()
        => ToLocalDateTime().ToString("d", CultureInfo.CurrentCulture);

    public string ToLocalTimeOnlyString()
        => ToLocalDateTime().ToString("t", CultureInfo.CurrentCulture);

    public string ToLocalDateTimeString()
        => ToLocalDateTime().ToString("g", CultureInfo.CurrentCulture);

    public static DateTimeNumbers GlobalNow
    {
        get
        {
            return new DateTimeNumbers(DateTime.UtcNow);
        }
    }
}