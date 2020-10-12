using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.GameState
{
    public class CurrentEvent
    {
        public static bool Active {  get { return Game1.CurrentEvent != null; } }
        public static bool Skippable { get { return Game1.CurrentEvent.eventCommands.Contains("skippable"); } }
        public static string CurrentCommand
        {
            get
            {
                var currentEvent = Game1.CurrentEvent;
                return currentEvent.eventCommands[currentEvent.CurrentCommand];
            }
        }
        public static string LastCommand
        {
            get
            {
                var currentEvent = Game1.CurrentEvent;
                if (currentEvent.CurrentCommand > 0)
                    return currentEvent.eventCommands[currentEvent.CurrentCommand-1];
                return "";
            }
        }
    }
}
