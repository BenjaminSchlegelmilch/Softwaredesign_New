using System;

public struct TimeSpanDay{
    public DateTime start;
    public DateTime end;
    public Days dayName;

    public TimeSpanDay(DateTime start, DateTime end, Days dayName) 
    {
        this.start = start;
        this.end = end;
        this.dayName = dayName;
    }

}