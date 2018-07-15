using System;
using System.Collections.Generic;

namespace Abschlussaufgabe___TextAdventure
{
    class NPC: Creature
    {
        public bool IsActive {get; private set;}
        public bool IsAggressive {get; private set;}
        public List<CreatureDialogLine> DialogLines {get; set;} = new List<CreatureDialogLine>();
        public int DialogPhase {get; private set;}
        public int MaxDialogPhase {get; private set;}
        public bool CanSpeak {get; private set;}

        public NPC (string name, string description, int health, int damage, bool isActive, bool isAggressive, bool canSpeak){
            Name = name;
            Description = description;
            Health = health;
            Damage = damage;
            IsActive = isActive;
            IsAggressive = isAggressive;
            CanSpeak = canSpeak;
        }
    }
}