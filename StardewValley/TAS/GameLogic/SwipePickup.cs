using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Tools;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;
using TAS.Inputs;
using TAS.Utilities;

namespace TAS.GameLogic
{
    public class SwipePickup : IGameLogic
    {
        public override string Name => "SwipePickup";
        private bool SwungLastFrame = false;
        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);
            if (!Player.Active)
                return false;
            
            kstate = new SKeyboardState();
            // auto cancel out
            if (SwungLastFrame)
            {
                kstate = new SKeyboardState();
                kstate.Add(Keys.RightShift);
                kstate.Add(Keys.Delete);
                kstate.Add(Keys.R);
                SwungLastFrame = false;
                return true;
            }
            // are we harvesting?
            if (Player.IsHarvestingItem)
            {
                bool hasTool = ToolUsage.GetToolKey<MeleeWeapon>(out kstate);
                if (!hasTool)
                {
                    // don't auto advance, you may want to do more complex things here
                    return false;
                }
                // on the correct tool
                if (kstate == null)
                {
                    kstate = new SKeyboardState("C");
                    SwungLastFrame = true;
                }
                else
                {
                    // kstate contains the current keyboard command to swap to the tool
                }
                return true;
            }
            SwungLastFrame = false;
            return false;

        }

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: Auto swipe pickup if melee weapon in inventory", Name)
            };
        }
    }
}
