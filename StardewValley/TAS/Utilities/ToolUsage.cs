using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;
using TAS.Inputs;

namespace TAS.Utilities
{
    public class ToolUsage
    {
        public static int DistanceThreshold = 12;
        public static bool GetToolKey<T>(out SKeyboardState kstate)
        {
            kstate = null;
            if (Player.CurrentTool is T)
                return true;

            kstate = new SKeyboardState();
            for(int i = 0; i < Game1.player.MaxItems; ++i)
            {
                if (Game1.player.items[i] is T)
                {
                    if (i < 12)
                    {
                        switch (i)
                        {
                            case 10:
                                kstate.Add("OemMinus");
                                break;
                            case 11:
                                kstate.Add("OemPlus");
                                break;
                            default:
                                kstate.Add("D" + (i + 1).ToString());
                                break;
                        }
                    }
                    else if (i < 24)
                    {
                        kstate.Add("LeftControl");
                        kstate.Add("Tab");
                    }
                    else
                    {
                        kstate.Add("Tab");
                    }
                    return true;
                }
            }
            return false;
        }

        public static bool UseTool<T>(int mouseX, int mouseY, out SKeyboardState kstate, out SMouseState mstate)
        {
            mstate = null;
            bool hasTool = GetToolKey<T>(out kstate);
            if (!hasTool)
                return false;
            
            // we are on the correct tool now
            if (kstate == null)
                return LeftClick(mouseX, mouseY, out mstate);

            // we need to swap to the right tool, make sure mouse is in correct place
            mstate = new SMouseState(mouseX, mouseY, false, false);
            return true;
        }

        public static bool RightClick(int mouseX, int mouseY, out SMouseState mstate)
        {
            if (Math.Abs(Controller.LastFrameMouse().MouseX - mouseX) > DistanceThreshold &&
                    Math.Abs(Controller.LastFrameMouse().MouseY - mouseY) > DistanceThreshold)
            {
                mstate = new SMouseState(mouseX, mouseY, false, false);
            }
            else
            {
                mstate = new SMouseState(mouseX, mouseY, false, true);
            }
            return true;
        }

        public static bool LeftClick(int mouseX, int mouseY, out SMouseState mstate)
        {
            if (Math.Abs(Controller.LastFrameMouse().MouseX - mouseX) > DistanceThreshold &&
                    Math.Abs(Controller.LastFrameMouse().MouseY - mouseY) > DistanceThreshold)
            {
                mstate = new SMouseState(mouseX, mouseY, false, false);
            }
            else
            {
                mstate = new SMouseState(mouseX, mouseY, true, false);
            }
            return true;
        }
    }
}
