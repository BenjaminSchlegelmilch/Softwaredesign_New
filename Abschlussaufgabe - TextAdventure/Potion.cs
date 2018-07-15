using System;

namespace Abschlussaufgabe___TextAdventure
{
    class Potion: Item
    {
        public int Cure {get; private set;}

        public Potion (string name, string description, bool carryable, int cure): base (name,  description, carryable)
        {
            Name = name;
            Description = description;
            Carryable = carryable;
            Cure = cure;        
        }
    }
}