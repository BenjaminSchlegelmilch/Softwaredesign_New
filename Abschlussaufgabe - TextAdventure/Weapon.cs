using System;

namespace Abschlussaufgabe___TextAdventure
{
    class Weapon: Item
    {
        public int Damage {get; private set;}

        public Weapon (string name, string description, bool carryable, int damage): base (name,  description, carryable)
        {
            Name = name;
            Description = description;
            Carryable = carryable;
            Damage = damage;        
        }
    }
}