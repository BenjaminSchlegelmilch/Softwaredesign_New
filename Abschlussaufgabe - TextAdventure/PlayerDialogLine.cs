using System;

namespace Abschlussaufgabe___TextAdventure
{
    class PlayerDialogLine: CreatureDialogLine
    {
        public int LineNumber {get; private set;}
        public int NewPhase {get; private set;}

        public PlayerDialogLine (string line, int dialogPhase, Item gift, int lineNumber, int newPhase): base (line, dialogPhase, gift)
        {
            Line = line;
            DialogPhase = dialogPhase;
            Gift = gift;
            LineNumber = lineNumber;
            NewPhase = newPhase;
        }
    }
}