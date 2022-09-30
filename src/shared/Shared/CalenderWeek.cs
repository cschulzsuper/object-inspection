using System;
using System.Globalization;

namespace ChristianSchulz.ObjectInspection.Shared;

public struct CalenderWeek
{
    public CalenderWeek(DateTime date)
    {
        Year = ISOWeek.GetYear(date);
        Week = ISOWeek.GetWeekOfYear(date);
    }

    public CalenderWeek(int year, int week)
    {
        Year = year;
        Week = week;
    }

    public int Year { get; init; }
    public int Week { get; init; }

    public void Deconstruct(out int year, out int week)
    {
        year = Year;
        week = Week;
    }

    public static CalenderWeek Zero
    {
        get
        {
            return new CalenderWeek(0, 0);
        }
    }
}