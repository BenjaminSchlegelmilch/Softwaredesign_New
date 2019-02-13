using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq; //Lambda
using System.Threading.Tasks; //Multithrat
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TimetableGenerator {
    public static List<Day> week; //Listen für json dateien
    public static TimeSpan[] times;
    public static List<Course> courses;
    public static List<OptionalCourse> optionalCourses;
    public static List<Room> rooms;

    public static List<Prof> profs;
    public static List<CourseOfStudy> coursesOfStudy;

    static void Main (string[] args) { //Methodenaufrufe der Jsons 
        LoadJsonRooms ();
        LoadJsonProfs ();
        LoadJsonCoursesOfStudy ();
        LoadJsonCourses ();
        LoadOptionalCourses (); //Unbenennen --> Hier werden alle JSON Daten geladen.
       
        LoadMandatoryCourses ();//Hier werden die Plichtkurse zugewisen
        LoadProfsToCourses (); //Profs werden Kurse zugewiesen

        Task t = Task.Factory.StartNew(() => { //Threat wird gestartet --> Unter Funktionen werden aufgerufen. 
        Initializer (); //Hier weden unsere funktionsmethoden aufgerufen
        GenerateOptionalTimetable ();
        GenerateTimetable ();
        SortBy ();
        
        });
        t.Wait(new System.TimeSpan(0, 5, 0)); //Thrat soll 5 Minuten warten, wenn er danach noch läuft dann soll das Programm beendet werden.
        if (t.Status == TaskStatus.Running)
        {
            Console.WriteLine("Das Programm läuft schon zu lange und wurde beenden...");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }

    public static void Initializer () {

        week = new List<Day> (); //Muss immer per Hand eingetragen werden, welcher Tag es ist --> WOche ist ne Liste an Tagen, Objekte werden zugewiesen
        week.Add (new Day (Days.Montag));
        week.Add (new Day (Days.Dienstag));
        week.Add (new Day (Days.Mittwoch));
        week.Add (new Day (Days.Donnerstag));
        week.Add (new Day (Days.Freitag));

        times = new TimeSpan[6]; //Wird per Hand zugewiesen, welche Zeiten welcher Block hat 
        times[0] = new TimeSpan (new DateTime (2019, 01, 01, 7, 45, 0), new DateTime (2019, 01, 01, 9, 15, 0));
        times[1] = new TimeSpan (new DateTime (2019, 01, 01, 9, 30, 0), new DateTime (2019, 01, 01, 11, 0, 0));
        times[2] = new TimeSpan (new DateTime (2019, 01, 01, 11, 15, 0), new DateTime (2019, 01, 01, 12, 45, 0));
        times[3] = new TimeSpan (new DateTime (2019, 01, 01, 14, 0, 0), new DateTime (2019, 01, 01, 15, 30, 0));
        times[4] = new TimeSpan (new DateTime (2019, 01, 01, 15, 45, 0), new DateTime (2019, 01, 01, 17, 15, 0));
        times[5] = new TimeSpan (new DateTime (2019, 01, 01, 17, 30, 0), new DateTime (2019, 01, 01, 19, 0, 0));
        
        foreach (Day day in week) { //Für eintrag in der liste week (Foreach, vorne in Datenblock)
            day.blocks = new List<Block> (); //Liste mit blöckn wird erstellt (??) 
            for (int i = 0; i < times.Length; i++) { //FOrschleifen generriert Tage 
                day.blocks.Add (new Block (times[i], i+1, day.dayName)); //Add zu Liste: Zeit, block, Tag (Wird in konstruktor erstellt)
            }
        }

    }

    public static void GenerateTimetable () {
        foreach (Day day in week) { //Für jeden Tag in der Liste von Tagen (week)
            foreach (Block block in day.blocks) { //Jeder der Blöcke im Tag wird durchgegagen.

                foreach (CourseOfStudy courseOfStudy in coursesOfStudy) { //Für jeden Studiengang (Prüft ob nur DM Kurse geladen werden) werden alle Kurse von ein getragenen studiengängen belegt? 
                   
                    foreach (Course course in courseOfStudy.mandatoryCourses) { //Ruft die Pflichtkurse der Studiengägne auf Bsp MIB 1 ist im ersten block besetzt
                        if (courseOfStudy.cosUsed == false){ //bool der blockt das Studiengang mehrfach genutzt wird.
                            if (course.courseUsed == false) { //blockt das Kurs mehrfach genutzt wird.
                            Helper.CountStudentAmount(course); //Helper sagt wie viele Teilnehmer der Kurs insgesammt hat. 
                            courseOfStudy.mandatoryCourses = courseOfStudy.mandatoryCourses.OrderByDescending (x => course.studentAmount).ToList(); //Pflchtkurse der Studiengänge werden Absteigend nach der Studentenanzahlt sortiert. (Unnötig, weil: wir immer nur einen Kurs haben --> heißt: Müssen nicht sortiert werden da wir immer nur einen Kurs behandelns) 
                                rooms.OrderBy (room => room.seatAmount); //Räume werden nach Seat amount sortiert. (wird anhand der Plätze sortiert)
                                foreach (Room room in rooms) { //Nachdem wir die Räume sortiert haben suchen wir einen Raum in RÄume
                                    if (course.room == null) { //Hat der Kurs schon einen Raum? (Sicherheitsabfrage)
                                        if (room.seatAmount >= course.studentAmount == true) {  //Wenn Anzahl der Sitzplätze größer oder gleich ist.
                                            if (room != null) { //Vorsicht besser als nachsicht
                                                if (Helper.ContainsAllItems<string> (room.roomEquipment, course.equipment) == true) { //Checkt ob Raum das Equipment hat was Kurs braucht
                                                    if (Helper.TimeCheck (block.timespan.start, block.dayName, course.prof) == true) { //Übergeben Blockzeten + Prof, weil hier gecheckt werden muss ob der Prof verhindert ist oder nicht  
                                                        if (course.prof.profUsed == false && room.roomUsed == false) {//Checkt ob prof und raum frei ist. 
                                                            course.room = room; //Kurs du findest in raum stadt.
                                                            course.courseUsed = true;
                                                            course.room.roomUsed = true;
                                                            course.prof.profUsed = true;
                                                            foreach(var coursesOfStudyOfCourse in course.coursesOfStudy){ //Kurse haben Liste mit Studiengängen die teilnehmen. Gehen die Studiengänge mit einem Kurs durch
                                                                foreach(var coursesOfStudy in coursesOfStudy){ //Wir schauen ob der Studiengang im Kurs mit einem tatsächlichen Studiengang
                                                                    if(coursesOfStudyOfCourse.name == coursesOfStudy.name){ //?? (Checkt die Studiiengänge ,miteinadner ab)
                                                                        coursesOfStudy.cosUsed = true;
                                                                    }
                                                                }
                                                            }
                                                            block.blockCourses.Add (course); //weist Block den Kurs zu den wir in Schleife haben
                                                        }
                                                    }
                                                } 
                                            }
                                        } 
                                    }
                                }
                            }
                            course.studentAmount = 0;
                        }
                    }
                }
                foreach (Course course in block.blockCourses) { //Hier wird wieder resettet, das der Raum und der Prof für den nächsten Block frei ist. 
                    course.room.roomUsed = false;//Wenn Kurs 1x used, dann ummer usesd
                    course.prof.profUsed = false;
                }
                foreach(var coursesOfStudy in coursesOfStudy) //Am Ende des Blocks wird StudienGANG wieder freigesetzt, da dieser dann wieder Zeit hat. 
                    coursesOfStudy.cosUsed = false;
            }
        }
    }

    public static void SortBy(){ //Nach generierung des Timetable
        Console.WriteLine("Wahlen Sie entweder Dozent, Raume, einen Studiengang(MIB,MKB,OMB) der Wahl mit Semester Bsp: 'OMB 4' oder nichts");
        Console.WriteLine(" ");
        string content = Console.ReadLine();
        string [] console = content.Split(' '); //String wird an dem leerzeichen gesplittet
        string con = console[0]; // an stellle 0 = MIB
        Console.WriteLine(" ");

        switch(con)
      {
        case "Dozent": //Ruft Funktion auf
            Console.WriteLine(" ");
            foreach(Prof prof in profs) { //Für jeden Prof jeder Tag (Wird nach den Professoren sortiert)
                foreach (Day day in week) {
                    foreach (Block block in day.blocks) {
                        foreach (Course course in block.blockCourses) { //Für jeden Kurs der in diesem Block stattfindet. 
                            if(prof == course.prof){ // ist der Prof der Prof von dem Kurs 
                                Console.WriteLine (course.prof.name + " :");
                                Console.WriteLine (course.name);
                                Console.WriteLine ("in " + course.room.name);
                                Console.WriteLine ("Am " + day.dayName);
                                Console.WriteLine ("im " + block.blockNumber + ". Block");
                                Console.WriteLine(" ");
                            }
                        }
                    }
                }
            }    
            break;
        case "Raume":
            Console.WriteLine(" ");
            foreach(Room room in rooms) {
                foreach (Day day in week) {
                    foreach (Block block in day.blocks) {
                        foreach (Course course in block.blockCourses) { //block.blockCourse für deden Kurs der liste im block.
                            if(room == course.room){ //Wenn Raum der selbe ist der bei Kurse hinterlegt ist
                                Console.WriteLine (course.room.name + ": ");
                                Console.WriteLine ("gehalten von " + course.prof.name);
                                Console.WriteLine (course.name);
                                Console.WriteLine ("Am " + day.dayName);
                                Console.WriteLine ("im " + block.blockNumber + ". Block");
                                Console.WriteLine(" ");
                            }
                        }
                    }
                }
            }    
            break;
        case "MKB":
        case "MIB":
        case "OMB":
            Helper.PrintCourseOfStudy(content); //Je nach case wird die fuktion aufgerufen und der gesammte content übvergeben
            break;
        default:
            Console.WriteLine("Gesamtstundeplan");
            Console.WriteLine(" ");
            foreach (Day day in week) {
                foreach (Block block in day.blocks) {
                    foreach (Course course in block.blockCourses) {
                        Console.WriteLine(" ");

                        Console.WriteLine (course.name);
                        Console.WriteLine (course.room.name);
                        Console.WriteLine (course.prof.name);
                        Console.WriteLine (day.dayName);
                        Console.WriteLine (block.blockNumber + ". Block");

                        foreach (var CoursesOfStudyOfCourse in course.coursesOfStudy){
                            if(course.coursesOfStudy.Count > 1)
                            Console.Write (CoursesOfStudyOfCourse.name + " ");
                        }
                        Console.WriteLine (" ");                        
                    }
                }
            }
            break;
        }
    }

    public static void GenerateOptionalTimetable (){ //Wird nach Initilizer aufgerufen
        foreach(OptionalCourse optionalCourse in optionalCourses){
            Helper.SetBlock(optionalCourse);
        }
    }
    public static void LoadJsonRooms () {
        var data = File.ReadAllText ("Rooms.json");//Json in var Gespeichert (noch unserialisiert)
        rooms = JsonConvert.DeserializeObject<List<Room>> (data); //Json wird in Liste eingetragen --> Wird deserialisiert 
    }

    public static void LoadJsonProfs () {
        var data = File.ReadAllText ("Profs.json");
        profs = JsonConvert.DeserializeObject<List<Prof>> (data);
    }

    public static void LoadJsonCourses () {
        var data = File.ReadAllText ("Module.json");
        courses = JsonConvert.DeserializeObject<List<Course>> (data);
    }

    public static void LoadJsonCoursesOfStudy () {
        var data = File.ReadAllText ("CoursesOfStudy.json");
        coursesOfStudy = JsonConvert.DeserializeObject<List<CourseOfStudy>> (data);
    }

    public static void LoadMandatoryCourses () {
        for (int i = 0; i < coursesOfStudy.Count; i++){ //gehen durch corsesOfStudy.count mit forschleife
            foreach(Course course in courses){ //alle Kurse werden duchgegagen
                for(int j = 0; j < coursesOfStudy[i].mandatoryCourses.Count; j++)
                    if(course.name == coursesOfStudy[i].mandatoryCourses[j].name) //Sobald ein ist werden die Daten zugewiesen  
                        coursesOfStudy[i].mandatoryCourses[j] = course;
                }    
            }
        }
    public static void LoadProfsToCourses () { 
        foreach (Course course in courses) {// Geht alles Kurese durch 
            foreach (Prof prof in profs) { // Geht für gefundenen Kurs alle Prof durch 
                foreach (string profCourse in prof.courses) { //????
                    if (profCourse == course.name)
                        course.prof = prof;
                }
            }
        }
    }

    public static void LoadOptionalCourses (){
        var data = File.ReadAllText ("OptionalCourse.json");
        optionalCourses = JsonConvert.DeserializeObject<List<OptionalCourse>> (data);
    }
}

//Sobald wir eine der if bedingungen niht erfüllt ist springt man zurück in die davorige foreach

//Programmablauf: Es werden zu erst die Klassenatribute festgelegt. #endregion
//Initilizer erstellt die Tage --> Blöcke --> Zeiten
//Danach werden die optionalen Kurse festgesetzt
//Danach werden Pflichtfächer generiert 