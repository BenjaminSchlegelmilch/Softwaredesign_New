using System;
using System.Collections.Generic;

public class Block{
    public TimeSpan timespan;
    public int blockNumber;
    public List<Course> blockCourses = new List<Course>();
    public Days dayName;

    public Block (TimeSpan zeit, int blockNumber, Days dayName) 
        {
            this.timespan = zeit;
            this.blockNumber = blockNumber;
            this.dayName = dayName;
        }
}