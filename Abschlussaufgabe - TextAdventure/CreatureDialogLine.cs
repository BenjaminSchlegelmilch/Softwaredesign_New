using System;

namespace Abschlussaufgabe___TextAdventure
{
    class CreatureDialogLine
    {
        public string Line {get; protected set;}
        public int DialogPhase {get; protected set;}
        public Item Gift {get; protected set;}

        public CreatureDialogLine (string line, int dialogPhase, Item gift)
        {
            Line = line;
            DialogPhase = dialogPhase;
            Gift = gift;     
        }
    }
}