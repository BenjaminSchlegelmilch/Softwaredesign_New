using System;
using System.Collections.Generic;

namespace Abschlussaufgabe___TextAdventure
{
    class Player: Creature
    {
        public List<PlayerDialogModel> Dialogs = new List<PlayerDialogModel>();
        public Room CurrentRoom {get; private set;}
        public int BaseDamage {get; private set;}
        public Weapon EquippedWeapon {get; private set;}
        public int MaxHealth {get; private set;}
        

        public Player (string name, string description, int maxHealth, int baseDamage, Room currentRoom){
            Name = name;
            Description = description;
            MaxHealth = maxHealth;
            Health = MaxHealth;
            BaseDamage = baseDamage;
            Damage = BaseDamage;
            CurrentRoom = currentRoom;
        }

        #region PlayerActions

        public void Help ()
        {
            Console.WriteLine("You can use the following commands to play the game:");
            Console.WriteLine("'inventory' - to check your pockets");
            Console.WriteLine("'look' - to look around in the room you're in");
            Console.WriteLine("'lookat <name of something in the room / inventory>' - to look at the object or person ");
            Console.WriteLine("'take <name of an item in the room>' - to pick an item up");
            Console.WriteLine("'drop <name of an item in your inventory' - to drop an item from your inventory");
            Console.WriteLine("'use' - to take a potion / eat food or equip a weapon");
            Console.WriteLine("'go <direction / first letter of direction>' - move to the direction (north, east, south or west)");
            Console.WriteLine("'speak <person>' - to start a conversation with the person");
            Console.WriteLine("'speak <creature / person>' - to start a fight with the creature or person");
            Console.WriteLine("'info' - show info about the Player");
            Console.WriteLine("'quit' - to quit the game");
        }

        public void PlayerInfo ()
        {
            Console.WriteLine("Your stats:");
            Console.WriteLine("Health: " + Health + " / " + MaxHealth);
            Console.WriteLine("Damage: " + Damage + " (= base damage + weapon damage)");
            if(EquippedWeapon != null)
            {
                Console.WriteLine("Equipped weapon: " + EquippedWeapon.Name);
                Console.WriteLine("Equipped weapon damage: " + EquippedWeapon.Damage);
            }
            else
                Console.WriteLine("Equipped weapon: none");  
        }

        public void CheckInventory ()
        {
            Console.WriteLine("Your stuff:");
            foreach (Item item in Inventory)
            {
                Console.WriteLine(item.Name);
            }
        }

        public void Look ()
        {
            Console.WriteLine("You are looking around in the " + CurrentRoom.Name + ":");
            Console.WriteLine(CurrentRoom.Description);
            Console.WriteLine(Environment.NewLine + "The following places are nearby:");
            foreach(KeyValuePair<TextAdventure.Direction, Room> neighbor in CurrentRoom.Neighbors)
            {
                Console.WriteLine("   You can reach the " + neighbor.Value.Name + " in the " + neighbor.Key + ".");
            }
            Console.WriteLine(Environment.NewLine + "The persons in the location are:");
            foreach (NPC npc in CurrentRoom.NPCs)
            {
                Console.WriteLine("   " + npc.Name);
            }
            Console.WriteLine(Environment.NewLine + "Also you can see the following things nearby:");
            foreach (Item item in CurrentRoom.Items)
                Console.WriteLine("   " + item.Name);
        }

        public void LookAt (string thing)
        {
           if (String.IsNullOrWhiteSpace(thing))
                Console.WriteLine ("Please select a Object to look at.");
            else
            {
                GameObject subject = CurrentRoom.Items.Find(x => x.Name.ToLower() == thing);
                if (subject == null)
                    subject = CurrentRoom.NPCs.Find(x => x.Name.ToLower() == thing);
                if(subject == null)
                    subject = Inventory.Find(x => x.Name.ToLower() == thing);
                if(subject == null)
                    Console.WriteLine("There is nothing with the name '" + thing + "' in the room.");
                else
                {
                    Console.WriteLine(subject.Description);
                    if(subject is Weapon)
                    {
                        Weapon weapon = (Weapon) subject;
                        Console.WriteLine(weapon.Name + " can be used as a weapon and causes " + weapon.Damage + " damage.");
                    } 
                    else if (subject is Potion)
                    {
                        Potion potion = (Potion) subject;
                        Console.WriteLine(potion.Name  + " can be consumed and is capable of curing " + potion.Cure + " healthpoints.");
                    } 
                }
                
            } 
        }

        public void Attack (string person)
        {
           if (String.IsNullOrWhiteSpace(person))
                Console.WriteLine ("Please select a Object to look at.");
            else
                if(CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person) != null)
                    if(CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person).Health == 0)
                        Console.WriteLine("You can't attack corpses. But you can loot them.");
                    else
                        Fight(TextAdventure.Player, CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person));
                else if (CurrentRoom.Items.Find(x =>  x.Name.ToLower() == person) != null)
                    Console.WriteLine ("You can't fight this...");
                else
                    Console.WriteLine ("There is no " + person + " nearby.");
        }

        public void SpeakTo (string person)
        {
           if (String.IsNullOrWhiteSpace(person))
                Console.WriteLine ("Please select a Person to speak to.");
            else
                if(CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person) != null)
                    if(CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person).Health == 0)
                        Console.WriteLine("You can't speak to corpses. But you can loot them.");
                    else
                        Dialog(TextAdventure.Player, CurrentRoom.NPCs.Find(x =>  x.Name.ToLower() == person));
                else if (CurrentRoom.Items.Find(x =>  x.Name.ToLower() == person) != null)
                    Console.WriteLine ("You can't talk to this...");
                else
                    Console.WriteLine ("There is no " + person + " nearby.");
        }

        public void Move (string direction)
        {
            Room nextRoom = null;
            bool validDirection = false;
            TextAdventure.Direction selectedDirection = TextAdventure.Direction.north;

            switch (direction)
            {
                case "north": case "n":
                    CurrentRoom.Neighbors.TryGetValue(TextAdventure.Direction.north, out nextRoom);
                    validDirection = true;
                    selectedDirection = TextAdventure.Direction.north;
                    break;
                case "south": case "s":
                    CurrentRoom.Neighbors.TryGetValue(TextAdventure.Direction.south, out nextRoom);
                    validDirection = true;
                    selectedDirection = TextAdventure.Direction.south;
                    break;
                case "east": case "e":
                    CurrentRoom.Neighbors.TryGetValue(TextAdventure.Direction.east, out nextRoom);
                    validDirection = true;
                    selectedDirection = TextAdventure.Direction.east;
                    break;
                case "west": case "w":
                    CurrentRoom.Neighbors.TryGetValue(TextAdventure.Direction.west, out nextRoom);
                    validDirection = true;
                    selectedDirection = TextAdventure.Direction.west;
                    break;
                default:
                    Console.WriteLine("'" + direction + "'" + " is not a valid direction to go.");
                    break;
            }

            if (validDirection && nextRoom == null)
                Console.WriteLine("There is no place to go in the " + selectedDirection.ToString() + ".");
            else if (nextRoom != null)
            {
                CurrentRoom = nextRoom;
                Console.WriteLine("You are now in the " + CurrentRoom.Name + ".");
                Look();
                if(!CurrentRoom.AlreadyVisited)
                    CurrentRoom.StartUp();
            }    
        }

        public void Take (string thing)
        {
            if (String.IsNullOrWhiteSpace(thing))
                Console.WriteLine ("Please select a Object to pick up.");
            else
            {
                Item item = CurrentRoom.Items.Find(x => x.Name.ToLower() == thing);
                if(!item.Carryable)
                    Console.WriteLine("Did you really try to pick up a " + item.Name + " ?");
                else
                {
                    if (item == null)
                        Console.WriteLine("There is no item with the name '" + thing + "' in the room.");
                    else
                    {
                        Inventory.Add(item);
                        CurrentRoom.Items.Remove(item);
                        Console.WriteLine("You added a " + item.Name + " to your inventory.");
                    }
                }
            }
        }

        public void Drop (string thing)
        {
            if (String.IsNullOrWhiteSpace(thing))
                Console.WriteLine ("Please select a object to drop.");
            else
            {
                Item item = Inventory.Find(x => x.Name.ToLower() == thing);
                if (item == null)
                    Console.WriteLine("There is no item with the name '" + thing + "' in your inventory.");
                else
                {
                    CurrentRoom.Items.Add(item);
                    Inventory.Remove(item);
                    Console.WriteLine("You dropped a " + item.Name + " from your inventory.");
                }
            }
        }

        public void Use (string thing)
        {
            if (String.IsNullOrWhiteSpace(thing))
                Console.WriteLine ("Please select a object to use.");
            else
            {
                Item item = Inventory.Find(x => x.Name.ToLower() == thing);
                if (item == null)
                    Console.WriteLine("There is no item with the name '" + thing + "' in your inventory.");
                else
                {
                    if(item is Weapon)
                    {
                        Weapon newWeapon = (Weapon) item;
                        EquippedWeapon = newWeapon;
                        Damage = BaseDamage + newWeapon.Damage;
                        Console.WriteLine("You equipped " + newWeapon.Name + " as your Weapon. Your damage is now " + Damage + ".");
                    } 
                    else if (item is Potion)
                    {
                        Potion potion = (Potion) item;
                        Health += potion.Cure;
                        if(Health >= MaxHealth)
                            Health = MaxHealth;
                        Console.WriteLine("You consumed " + potion.Name  + ". You now have " + Health + " healthpoints.");
                    }
                    else
                        Console.WriteLine("You can't use this.");
                }
            }
        }

        #endregion

        #region Helper
        #endregion
    }
}