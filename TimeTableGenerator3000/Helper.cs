using System;
using System.Collections.Generic;
using System.Linq;

public static class Helper {

    public static bool ContainsAllItems<T> (this IEnumerable<T> a, IEnumerable<T> b) { //wenn a MUSS alles aus b haben (Haben wir gegoogelt)
        return !b.Except (a).Any (); //bool wird zurückgegeben (b hat welche die a nicht hat) (Gleicht Equipment ab)
    }

    public static void CountStudentAmount (Course course) {
        foreach (CourseOfStudy courseOfStudyOfCourse in course.coursesOfStudy) { //für jeden Studiengang der diesen Kurs besucht
            foreach (CourseOfStudy courseOfStudy in TimetableGenerator.coursesOfStudy) { //gehen alle Studiengänge durch
                if (courseOfStudyOfCourse.name == courseOfStudy.name)
                    course.studentAmount = course.studentAmount + courseOfStudy.studentAmount;
            }
        }
    }

    public static CourseOfStudy MapCourseOfStudyNameToCourseOfStudy (string courseOfStudyName) { //Datentyp ist studiengang der zurück gegeben wird
        foreach (CourseOfStudy courseOfStudy in TimetableGenerator.coursesOfStudy) { //geh alle studiengänge durch und schaut ob der Studiengang übereinstimmt
            if (courseOfStudyName == courseOfStudy.name) //der übergebene String wird mit dem Parameter name von Course of Study abgeglichen
                return courseOfStudy; // es wird der courseOf Study mit dem übereinstimmenden Name übergeben.
        }
        return null;
    }

    public static bool TimeCheck (DateTime dateTime, Days day, Prof prof) { //Prüft ob Prof Zeit hat.
        foreach (TimeSpanDay timeSpanDay in prof.occupied) {
            if (timeSpanDay.start < dateTime && timeSpanDay.end > dateTime && day == timeSpanDay.dayName) //Prüft ob Prof in der Zeitspanne kann und am selben tag
                return false;
        }
        return true;
    }

    public static void SetBlock (OptionalCourse optionalCourse) { //Zuweisungslogik der optional Kurses
        foreach (Day day in TimetableGenerator.week) { //Für jeden Tag in der Woche
            if (day.dayName == optionalCourse.timeSpanDay.dayName) { // Name des Tages der gleiche wie Tag des optionalen Kurs
                for (int i = 0; i < TimetableGenerator.times.Length; i++) { //Wir gehen die Uhrzeiten in den blöcken durch, wir gehen alle Zeiten durch um heruaszufinden wann der Block stattfindet
                    if (TimetableGenerator.times[i].start == optionalCourse.timeSpanDay.start) { //Ist der block an der stelle i der gleiche wie der an der WPV Versanstatlung die stattfindet.
                        TimeSpan timespan = new TimeSpan (TimetableGenerator.times[i].start, TimetableGenerator.times[i].end); //WPV wird hier festgesetzt: Für den Block vrauchen wir eine Timespan
                        Block block = new Block (timespan, i, day.dayName); //Attribute für Block wird zugewiesen. (brauchen bei der Erstellung ders Blocks einen Timespan --> Konstruktor erwartet dieses argument.
                        Course course = new Course (); //Neuer Kurs wird erstellt 
                        course.name = optionalCourse.name;
                        course.prof = optionalCourse.prof;
                        course.room = optionalCourse.room;
                        block.blockCourses.Add (course); //Block Kurse erwartet nur Daten vom Typ Kurs.
                        day.blocks[i] = block; //Block in der Zeitspanne wird als block festgelegt
                        foreach (Prof prof in TimetableGenerator.profs) { //geht alle Prof durch 
                            if (optionalCourse.prof.name == prof.name) //Sucht richtigen Prof wenn Name übereinstimmt
                                prof.occupied.Add (optionalCourse.timeSpanDay); //Adde Zeit zu Prof seinen gesperrten Zeiten 
                        }
                    }
                }
            }
        }
    }
    public static void PrintCourseOfStudy (string courseofStudyName) {
        Console.WriteLine (courseofStudyName + " :");
        Console.WriteLine (" ");
        CourseOfStudy courseOfStudy = MapCourseOfStudyNameToCourseOfStudy (courseofStudyName); //Speichert wert der MEthode in der Variableder MEthode in der Variable (der mitgegebene Name wird mit dem Course of Study name abgeglichen)
        if (courseOfStudy != null) { //Check ob Studiengang gewählt ist 
            foreach (Day day in TimetableGenerator.week) { //Für jeden Tag
                foreach (Block block in day.blocks) { //Für jeden Block
                    foreach (Course blockCourseCourse in block.blockCourses) { //Für jeden Kurs der in diesem Block stattfindet
                        foreach (Course courseOfMandatoryCourses in courseOfStudy.mandatoryCourses) { //Für jeden Pflichtkurs in den Pflichtkursen
                            if (blockCourseCourse == courseOfMandatoryCourses) { // Kurse in den blöcken werden mit dem selektierten Studiengang (z.b. MIB 1 abgeglichen)
                                Console.WriteLine (courseOfMandatoryCourses.name);
                                Console.WriteLine (courseOfMandatoryCourses.room.name + ": ");
                                Console.WriteLine ("gehalten von " + courseOfMandatoryCourses.prof.name);
                                Console.WriteLine ("Am " + day.dayName);
                                Console.WriteLine ("im " + block.blockNumber + ". Block");
                                Console.WriteLine (" ");
                            }
                        }
                    }
                }
            }
            PrintPossibleOptionalCourses (courseOfStudy); //wenn Eingabe erfolgreich ruf diese Funktion auf 
        } else
            Console.WriteLine ("Falsche Eingabe, oder Studiengang nicht vorhanden, Bsp: 'MIB 2'");
    }

    public static void PrintPossibleOptionalCourses (CourseOfStudy courseOfStudy) { //Gibt unseren gewählten studiengang mit | Funktion soll sagen welche WPV's UNSER studiengang besucht werden soll.
        bool key = true; //unnötig
        Console.WriteLine ("Mit diesem Stundenplan, koenntest du folgende Wahlpflichtkurse belegen");
        Console.WriteLine (" ");
        foreach (Day day in TimetableGenerator.week) { //Für jeden Tag in der woche
            foreach (Block block in day.blocks) { //durch alle Blöcke im Tag
                foreach (OptionalCourse optionalCourse in TimetableGenerator.optionalCourses) { //Geht mit jedem optional Kurs der gesammten optinal Kurse durch 
                    if (block.blockCourses.Count == 0 && optionalCourse.optionalCourseUsed == false) { //Hat der block überhaupt Kurse wenn 0 (== 0), dann Liste leer --> ist das WPV schon genutzt? (bool) 
                        if (CheckBlockToCourse (block, optionalCourse.timeSpanDay)) { //checkt ob bool true ist
                           
                            optionalCourse.optionalCourseUsed = true;
                            PrintOptionalCourseData (block, day, optionalCourse);
                        }
                    } else {
                        foreach (Course course in block.blockCourses) { //Jeder Kurs im Block der Blockkurse
                            if (block.blockCourses.Count == 0) //Sicherheitsanfrage 
                                break; //geht zurück in die oberste foreach
                            key = true; //weil 104 auf false gesetzt wird
                            for (int i = 0; i < course.coursesOfStudy.Count; i++) { //Checkt die anderen Studiengägne (bzw ob einer davon)
                                CourseOfStudy courseOfStudyOfCourses = course.coursesOfStudy[i]; //i = coursesOfStudy an stelle i 0 = MIB. 1=OMB, 2=MKB (reihenfolge ist nicht festgelegt)
                                if (courseOfStudyOfCourses.name == courseOfStudy.name || course.coursesOfStudy == null) { // Hat der Studiengang gerade unterricht --> (courseOfStudy.name ist die eingabe.) || der Studiengang der in diesem Kurs ist == null (JSON fall, falls vergessen werden die Daten einzutragen.); 
                                    key = false; //Key false --> haben untericht 
                                }
                            }

                            if (key && optionalCourse.optionalCourseUsed == false) { // key = (key == true), ist kurs schon genutzt
                                if (CheckBlockToCourse (block, optionalCourse.timeSpanDay)) { //checkt ob bool in der funktion true ist
                                    optionalCourse.optionalCourseUsed = true;
                                    PrintOptionalCourseData (block, day, optionalCourse); //wenn alles funktioniert wird Optional Kurs ausgegeben
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static bool CheckBlockToCourse (Block block, TimeSpanDay timeSpanDayCourse) { //von wann bis wann an welchem tag findet der Kurs statt (haben nur wpvs) JSON hat diese struktur, die Parameter werden in der Funktion
        if (block.timespan.start == timeSpanDayCourse.start && //starten diese Kurs gleichzeitg mit block und wpv (timeSpanDayCourse ist optionalTimeSpanDay)
            block.dayName == timeSpanDayCourse.dayName) { //sind die am gleichen tag (block und wpv)
            return true; //wenn gleich dann true
        }
        return false;
    }

    public static void PrintOptionalCourseData (Block block, Day day, OptionalCourse optionalCourse) { //Print funktion
            int i = block.blockNumber + 1;
            Console.WriteLine (optionalCourse.name);
            Console.WriteLine (optionalCourse.room.name + ": ");
            Console.WriteLine ("gehalten von " + optionalCourse.prof.name);
            Console.WriteLine ("Am " + day.dayName);
            Console.WriteLine ("im " + i + ". Block");
            Console.WriteLine (" ");
        }
}

// Optionales Kurse werden zu erst gesetzt, dann werden die Pflichtkurse gesetzt