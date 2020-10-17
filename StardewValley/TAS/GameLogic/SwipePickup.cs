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
                kstate = new SKeyboardState();
                if (!(Player.CurrentTool is MeleeWeapon))
                {
                    // flip tool if possible
                    for(int i = 0; i < Game1.player.maxItems; i++)
                    {
                        if (Game1.player.items[i] is MeleeWeapon)
                        {
                            if (i < 12)
                            {
                                Keys key;
                                if (i == 10)
                                    key = Keys.OemMinus;
                                else if (i == 11)
                                    key = Keys.OemPlus;
                                else
                                    key = (Keys)Enum.Parse(typeof(Keys), string.Format("D{0}",(i+1) % 10));
                                kstate.Add(key);
                                return true;
                            }
                            else if (i < 24)
                            {
                                kstate.Add(Keys.LeftControl);
                                kstate.Add(Keys.Tab);
                                return true;
                            }
                            else if (i < 36)
                            {
                                kstate.Add(Keys.Tab);
                                return true;
                            }
                        }
                    }
                    // don't auto advance, you may want to do more complex things here
                    return false;
                }
                else
                {
                    // swing to trigger the cancel pickup
                    kstate.Add(Keys.C);
                    SwungLastFrame = true;
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
