using System;
using System.Collections.Generic;

namespace L07
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class Person 
    {
        public string name;
        public int age;
    }

    class Member: Person
    {
        public int MatriculationNumber;

        public List<Course> Courses;
    }

    class Lecturer: Person
    {
        public string Office;
        public string Consulation;

        public List<Course> Courses;

        public void ShowCourse ()
        {
            foreach (Course course in Courses)
                Console.WriteLine(course.Titel);
        }

        public List<Member> returnMember()
        {
            List<Member> allMembers = new List<Member>();
            
            foreach (Course course in Courses)
                foreach(Member member in course.Member)
                    if(!allMembers.Contains(member))
                        allMembers.Add(member);

            return allMembers;
        }
    }

    class Course
    {
        public string Titel;
        public string Meeting;
        public string Room;

        // Beziehungen 
        public Lecturer Lecturer;
        public List<Member> Member;

        public void ShowInfoText()
        {
            Console.WriteLine("The course " + Titel + " starts " + Meeting + " in" + Room + ".");
        }
    }
}