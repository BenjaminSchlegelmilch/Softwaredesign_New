using System;

public struct TimeSpan{
    public DateTime start;
    public DateTime end;
    public TimeSpan(DateTime start, DateTime end) 
    {
        this.start = start;
        this.end = end;
    }

}