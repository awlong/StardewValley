using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.GameState
{
    public class CurrentMenu
    {
        public static bool Active { get { return Game1.activeClickableMenu != null; } }

        // Save Game functions
        public static bool IsSaveGame { get { return Game1.activeClickableMenu is SaveGameMenu; } }
        public static bool CanQuit { get { return (Game1.activeClickableMenu as SaveGameMenu).quit; } }

        // Dialogue Box functions
        public static bool IsDialogue { get { return Game1.activeClickableMenu is DialogueBox; } }
        public static bool Transitioning
        {
            get
            {
                return Reflector.GetValue<DialogueBox, bool>(Game1.activeClickableMenu as DialogueBox, "transitioning");
            }
        }
        public static bool IsQuestion
        {
            get
            {
                return Reflector.GetValue<DialogueBox, bool>(Game1.activeClickableMenu as DialogueBox, "isQuestion");
            }
        }
        public static int CharacterIndexInDialogue
        {
            get
            {
                return Reflector.GetValue<DialogueBox, int>(Game1.activeClickableMenu as DialogueBox, "characterIndexInDialogue");
            }
        }
        public static int SafetyTimer
        {
            get
            {
                return Reflector.GetValue<DialogueBox, int>(Game1.activeClickableMenu as DialogueBox, "safetyTimer");
            }
        }
        public static string CurrentString
        {
            get
            {
                return (Game1.activeClickableMenu as DialogueBox).getCurrentString();
            }
        }
    }
}
