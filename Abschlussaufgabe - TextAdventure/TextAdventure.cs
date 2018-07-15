using System;
using System.Collections.Generic;
using System.Linq;

namespace Abschlussaufgabe___TextAdventure
{
    static class TextAdventure
    {
        public static Player Player {get; private set;}
        public static Item WinningItem {get; private set;}
        public static List<Room> Rooms {get; private set;} = new List<Room>();
        public static List<Item> Items {get; private set;} = new List<Item>();
        public static List<NPC> NPCs {get; private set;} = new List<NPC>();
        public static bool IsFinished {get; set;} = false;
        
        static void Main(string[] args)
        {
            LoadGameData();
            ConsoleWriteRed(Environment.NewLine + "Welcome to the Adventure of finding the best Fastfood meal in the world!" + Environment.NewLine);
            Console.WriteLine("Its 1 pm at the evening, you where the whole night at a party, and after all the alcohol you have only one gole in your mind, find the best meal you can imagine.");
            Console.WriteLine("You go to the mall next to the club, but you spent your whole money for beer and tequila and dont have any money in your wallet left'");
            Console.WriteLine("You now in the food court of your local shopping mall, lets start to find the best food for your hangover!.");

            for (;;)
            {
                if(Player.Inventory.Contains(WinningItem))
                {
                    ConsoleWriteGreen("You got the " + WinningItem.Name + " you finaly have your meal of the your dreams, enjoy!");
                    IsFinished = true;
                }

                if (IsFinished == true)
                {
                    ConsoleWriteRed("Thank you for playing.");
                    break;
                }

                ConsoleWriteDarkYellow("Type 'help' and press Enter if you don't know what to do." + Environment.NewLine);

                string input = Console.ReadLine().ToLower();

                string command = input.IndexOf(" ") > -1 
                  ? input.Substring(0, input.IndexOf(" "))
                  : input;

                string parameter = input.IndexOf(" ") > -1 
                  ? input.Substring(input.IndexOf(" ") + 1, input.Length - (input.IndexOf(" ") + 1))
                  : "";
                
                switch(command)
                {
                    case "quit": case "q":
                        IsFinished = true;
                        break;
                    case "help": case "h":
                        Player.Help();
                        break;
                    case "inventory": case "i":
                        Player.CheckInventory();
                        break;
                    case "look": case "l":
                        Player.Look();
                        break;
                    case "go": case "g":
                        Player.Move(parameter);
                        break;
                    case "take": case "t":
                        Player.Take(parameter);
                        break;
                    case "drop": case "d":
                        Player.Drop(parameter);
                        break;
                    case "lookat": case "la":
                        Player.LookAt(parameter);
                        break;
                    case "attack": case "a":
                        Player.Attack(parameter);
                        break;
                    case "info": case "in":
                        Player.PlayerInfo();
                        break;
                    case "use": case "u":
                        Player.Use(parameter);
                        break;
                    case "speak": case "s":
                        Player.SpeakTo(parameter);
                        break;
                    default:
                        ConsoleWriteRed("Unknown command!");
                        break;
                }
            }
        }

        public enum Direction {north=0, east=1, south=2, west=3};

        #region GameData

        public static void LoadGameData ()
        {

            #region Items

            Items.Add(new Item("Medicine", "Some Medicine for the stomach, maybe I need this!", true));
            Items.Add(new Item("Rotten Burger", "I don't think I should eat this anymore", false));
            Items.Add(new Item("Ketchup", "Ketchup without friese...", false));
            Items.Add(new Item("Soda Cup", "Nothing left anymore.", false));

            Items.Add(new Weapon("Extinguisher", "A extinguisher, maybe I will need this for some...danger.", true, 10));
            Items.Add(new Weapon("Kitchen Knife", "Damn, this knife is rally sharp.", true, 12));

            Items.Add(new Potion("Pumpkin Spice Java Chip Frappuccino", "Get some new energy from all that suger and coffein.", true, 50));
            Items.Add(new Potion("Small Fries", "Yuck! Cold and limp", true, 20));
            Items.Add(new Potion("Half Sandwich", "The other half is wrapped, should be okay .", true, 20));
            Items.Add(new Potion("Big Bacon Burger", "The grail of all burgers.", true, 150));
            Items.Add(new Potion("Big King", "Not bad for the beginning!.", true, 80));

            WinningItem = GetItemByName("Big Bacon Burger");

            #endregion

            #region Rooms

            Rooms.Add(new Room("Mc Donalds", "Just your local Mc Donalds, nothing special.", null));
            Rooms.Add(new Room("Subway", "To healty for me right now.", null));
            Rooms.Add(new Room("KFC", "Yummi Fried Chicken.", null));
            Rooms.Add(new Room("Burger King", "Flame Grilled, like I like it!.", null));
            Rooms.Add(new Room("Five Guys", "The place of the best burgers!", GetItemByName("Big Bacon Burger")));
            Rooms.Add(new Room("Starbucks", "Fresh Coffee, tasty!.", null));

            #endregion

            #region RoomNeighbors

            GetRoomByName("Mc Donalds").Neighbors.Add(Direction.west, GetRoomByName("Starbucks"));
            GetRoomByName("Mc Donalds").Neighbors.Add(Direction.south, GetRoomByName("Burger King"));

            GetRoomByName("Starbucks").Neighbors.Add(Direction.west, GetRoomByName("Mc Donalds"));
            GetRoomByName("Starbucks").Neighbors.Add(Direction.south, GetRoomByName("Subway"));

            GetRoomByName("Subway").Neighbors.Add(Direction.east, GetRoomByName("Burger King"));
            GetRoomByName("Subway").Neighbors.Add(Direction.north, GetRoomByName("Starbucks"));

            GetRoomByName("Burger King").Neighbors.Add(Direction.north, GetRoomByName("Mc Donalds"));
            GetRoomByName("Burger King").Neighbors.Add(Direction.east, GetRoomByName("Five Guys"));
            GetRoomByName("Burger King").Neighbors.Add(Direction.south, GetRoomByName("KFC"));
            GetRoomByName("Burger King").Neighbors.Add(Direction.west, GetRoomByName("Subway"));

            GetRoomByName("Five Guys").Neighbors.Add(Direction.west, GetRoomByName("Burger King"));

            GetRoomByName("KFC").Neighbors.Add(Direction.north, GetRoomByName("Burger King"));


            #endregion

            #region Player

            Player = new Player("Player", "That's me!", 100, 5, GetRoomByName("Starbucks"));

            #endregion

            #region AddItemsToRooms

            GetRoomByName("KFC").Items.Add(GetItemByName("Tray"));
            GetRoomByName("KFC").Items.Add(GetItemByName("Rotten Burger"));

            GetRoomByName("Subway").Items.Add(GetItemByName("Half Sandwich"));

            GetRoomByName("Starbucks").Items.Add(GetItemByName("Pumpkin Spice Java Chip Frappuccino"));

            GetRoomByName("Mc Donalds").Items.Add(GetItemByName("Small Fries"));
            GetRoomByName("Mc Donalds").Items.Add(GetItemByName("Medicine"));

            GetRoomByName("Burger King").Items.Add(GetItemByName("Soda Cup"));
            GetRoomByName("Burger King").Items.Add(GetItemByName("Ketchup"));

            GetRoomByName("Five Guys").Items.Add(GetItemByName("Big Bacon Burger"));

            #endregion

            #region NPCs

            NPCs.Add(new NPC("Staff", "He is not looking very well...", 100, 15, false, false, true));
            NPCs.Add(new NPC("Fat Guy", "I think this guy is here all day.", 100, 20, true, false, true));
            NPCs.Add(new NPC("Chef", "The master of this place.", 110, 8, true, true, true));

            #endregion

            #region AddItemsToInventories 
            
            GetNPCByName("Staff").Inventory.Add(GetItemByName("Big King"));
            GetNPCByName("Chef").Inventory.Add(GetItemByName("Kitchen Knife"));

            #endregion

            #region AddNPCsToRooms

            GetRoomByName("Burger King").NPCs.Add(GetNPCByName("Staff"));
            GetRoomByName("KFC").NPCs.Add(GetNPCByName("Fat Guy"));
            GetRoomByName("Five Guys").NPCs.Add(GetNPCByName("Chef"));

            #endregion

            #region NPCDialogLines

            GetNPCByName("Fat Guy").DialogLines.Add(new CreatureDialogLine("What do you want?!", 0, null));
            GetNPCByName("Fat Guy").DialogLines.Add(new CreatureDialogLine("Get away from my food", 1, null));
            GetNPCByName("Fat Guy").DialogLines.Add(new CreatureDialogLine("No!", 2, null));

            GetNPCByName("Chef").DialogLines.Add(new CreatureDialogLine("Noone is allowed to enter my kitchen!", 0, null));
            GetNPCByName("Chef").DialogLines.Add(new CreatureDialogLine("YOU WILL NEVER GET MY FOOD!", 1, null));
            GetNPCByName("Chef").DialogLines.Add(new CreatureDialogLine("NOONE IS ALLOWED TO ENTER!", 2, null));

            GetNPCByName("Staff").DialogLines.Add(new CreatureDialogLine("Hello welcome to Burger King how can I help you?", 0, null));
            GetNPCByName("Staff").DialogLines.Add(new CreatureDialogLine("You are at Burger King, i think we have some good food here.", 1, null));
            GetNPCByName("Staff").DialogLines.Add(new CreatureDialogLine("If you can find some medicine for me, maybe I can give you some free Food...", 2, null));
            GetNPCByName("Staff").DialogLines.Add(new CreatureDialogLine("Look at the other Burger Restaurants, people get there sick all the time!", 3, null));
            GetNPCByName("Staff").DialogLines.Add(new CreatureDialogLine("Maybe somwhere were you can find greasy food.", 4, null));
            GetNPCByName("Staff").DialogLines.Add(new CreatureDialogLine("Yes, come back if you found something, i can give you a bit food for it!", 5, null));
            GetNPCByName("Staff").DialogLines.Add(new CreatureDialogLine("Thank you bro! Here, some leftover food", 6, GetItemByName("Big King")));

            #endregion

            #region PlayerDialogModels

            Player.Dialogs.Add(new PlayerDialogModel(GetNPCByName("Fat Guy")));
            Player.Dialogs.Add(new PlayerDialogModel(GetNPCByName("Staff")));
            Player.Dialogs.Add(new PlayerDialogModel(GetNPCByName("Chef")));

            #endregion

            #region PlayerDialogLines

            GetPlayerDialogModelByDialogPartnerName("Fat Guy").DialogLines.Add(new PlayerDialogLine("Hello.", 0, null, 1, 1));
            GetPlayerDialogModelByDialogPartnerName("Fat Guy").DialogLines.Add(new PlayerDialogLine("Can I get something of your food? You dont look like you need it", 0, null, 2, 2));
            GetPlayerDialogModelByDialogPartnerName("Fat Guy").DialogLines.Add(new PlayerDialogLine("please!", 2, null, 1, 1));

            GetPlayerDialogModelByDialogPartnerName("Chef").DialogLines.Add(new PlayerDialogLine("Are you the Chef of this place?.", 0, null, 1, 1));
            GetPlayerDialogModelByDialogPartnerName("Chef").DialogLines.Add(new PlayerDialogLine("Please give me something to eat.", 0, null, 2, 2));
            
            GetPlayerDialogModelByDialogPartnerName("Staff").DialogLines.Add(new PlayerDialogLine("Im looking for a fantastic burger, do you know where i can get one?", 0, null, 1, 1));
            GetPlayerDialogModelByDialogPartnerName("Staff").DialogLines.Add(new PlayerDialogLine("You are looking not very well my friend", 0, null, 2, 2));
            GetPlayerDialogModelByDialogPartnerName("Staff").DialogLines.Add(new PlayerDialogLine("You dont look ver well, maybe you need some Medicine?", 1, null, 1, 5));
            GetPlayerDialogModelByDialogPartnerName("Staff").DialogLines.Add(new PlayerDialogLine("Where I can find it?", 2, null, 1, 3));
            GetPlayerDialogModelByDialogPartnerName("Staff").DialogLines.Add(new PlayerDialogLine("Where can i find something what can help you?", 3, null, 1, 4));
            GetPlayerDialogModelByDialogPartnerName("Staff").DialogLines.Add(new PlayerDialogLine("Here is your medicine, hope it helps.", 3, GetItemByName("Medicine"), 2, 6));
            GetPlayerDialogModelByDialogPartnerName("Staff").DialogLines.Add(new PlayerDialogLine("Got some medicine for you!. (Give Medicine to Staff)", 4, GetItemByName("Medicine"), 1, 6));

            #endregion
        }

        #endregion run

        #region Helper

        public static Room GetRoomByName(string name){
            return Rooms.Find(x => x.Name.ToLower() == name.ToLower());
        }

        public static Item GetItemByName(string name){
            return Items.Find(x => x.Name.ToLower() == name.ToLower());
        }



        public static NPC GetNPCByName(string name){
            return NPCs.Find(x => x.Name.ToLower() == name.ToLower());
        }

        public static PlayerDialogModel GetPlayerDialogModelByDialogPartnerName(string name){
            return Player.Dialogs.Find(x => x.DialogPartner.Name.ToLower() == name.ToLower());
        }

        #endregion

        #region GUIHelper

        public static void ConsoleWriteGreen (string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ConsoleWriteBlue (string input)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ConsoleWriteRed (string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ConsoleWriteDarkYellow (string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion
    }
}
