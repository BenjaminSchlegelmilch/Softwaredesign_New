using System.Collections.Generic;

public class Course{
    public string name;
    public string description;
    public Room room;
    public int studentAmount;
    public Prof prof;
    public Block block;
    public bool courseUsed;
    public List<string> equipment;
    public List<CourseOfStudy> coursesOfStudy = new List<CourseOfStudy>(); //???
}