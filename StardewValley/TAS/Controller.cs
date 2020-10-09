using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TAS.Inputs;
using TAS.Overlays;
using DateTime = StardewValley.DateTime;

namespace TAS
{
    public class Controller
    {
        public static Dictionary<string, IOverlay> Overlays;

        public static SaveState State;
        private static SMouseState Mouse;
        private static HashSet<Keys> RejectedKeys;
        private static HashSet<Keys> AddedKeys;
        static Controller()
        {

            Overlays = new Dictionary<string, IOverlay>();
            // TODO: use reflector to get all the types from the namespace
            IOverlay overlay = new DebugMouse();
            Overlays.Add(overlay.Name, overlay);

            State = new SaveState();
            Mouse = null;
            RejectedKeys = new HashSet<Keys>();
            AddedKeys = new HashSet<Keys>();
        }

        public static bool Update()
        {
            // get the actual input data loaded
            RealInputState.Update();
            
            // check if prior state or current keyboard should advance
            bool storedInputAdvance = HandleStoredInput();
            bool realInputAdvance = HandleRealInput();
            // TODO: only call when a menu is up?
            HandleTextBoxEntry();

            if (realInputAdvance && !storedInputAdvance)
            {
                // add the new frame data
                SInputState.SetMouse(RealInputState.mouseState, Mouse);
                SInputState.SetKeyboard(RealInputState.keyboardState, AddedKeys, RejectedKeys);

                State.FrameStates.Add(new FrameState(SInputState.GetKeyboard(), SInputState.GetMouse()));              
            }
            else if (storedInputAdvance)
            {
                // pull frame data from state list
                State.FrameStates[(int)DateTime.CurrentFrame].toStates(out SInputState.kState, out SInputState.mState);
            }
            // set flag to ensure input is pulled from state
            SInputState.Active = realInputAdvance || storedInputAdvance;
            return SInputState.Active;
        }

        public static void Draw()
        {
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            foreach (var overlay in Overlays.Values)
            {
                overlay.Draw();
            }
            Game1.spriteBatch.End();
        }

        private static bool HandleRealInput()
        {
            RejectedKeys.Clear();
            AddedKeys.Clear();
            Mouse = null;

            // frame advance
            bool advance = false;
            if (RealInputState.KeyTriggered(Keys.Q))
            {
                advance = true;
                RejectedKeys.Add(Keys.Q);
            }
            if (RealInputState.IsKeyDown(Keys.Space))
            {
                advance = true;
                RejectedKeys.Add(Keys.Space);
            }
            // exit
            if (RealInputState.IsKeyDown(Keys.OemOpenBrackets) && RealInputState.IsKeyDown(Keys.OemCloseBrackets))
            {
                // improve this logic?
                Game1.quit = true;
                advance = true;
            }
            // save/load
            if (RealInputState.KeyTriggered(Keys.OemPeriod))
                State.Save();
            if (RealInputState.KeyTriggered(Keys.OemComma))
                State = SaveState.Load(State.Prefix);
            return advance;
        }

        private static bool HandleStoredInput()
        {
            if (State.FrameStates.IndexInRange((int)DateTime.CurrentFrame))
            {
                return true;
            }
            return false;
        }

        private static void HandleTextBoxEntry()
        {
            TextBox textBox = TextBoxInput.GetSelected(out string Name);
            if (textBox != null)
            {
                if (textBox.Text.Length == 0)
                {
                    switch(Name)
                    {
                        case "nameBox":
                            TextBoxInput.Write(textBox, State.FarmerName);
                            break;
                        case "farmnameBox":
                            TextBoxInput.Write(textBox, State.FarmName);
                            break;
                        case "favThingBox":
                            TextBoxInput.Write(textBox, State.FavoriteThing);
                            break;
                        default:
                            TextBoxInput.Write(textBox, "abc");
                            break;
                    }
                }
            }
        }

       
    }
}
