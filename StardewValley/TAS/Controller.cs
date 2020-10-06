using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Inputs;
using TAS.Overlays;

namespace TAS
{
    public class Controller
    {
        public static Dictionary<string, IOverlay> Overlays;

        private static SMouseState Mouse;
        private static HashSet<Keys> RejectedKeys;
        private static HashSet<Keys> AddedKeys;
        static Controller()
        {

            Overlays = new Dictionary<string, IOverlay>();
            // TODO: use reflector to get all the types from the namespace
            IOverlay overlay = new DebugMouse();
            Overlays.Add(overlay.Name, overlay);

            Mouse = null;
            RejectedKeys = new HashSet<Keys>();
            AddedKeys = new HashSet<Keys>();
        }

        public static bool Update()
        {
            // get the actual input data loaded
            RealInputState.Update();

            // TODO: some type of logic goes in this function

            bool inputAdvance = HandleRealInput();
            if (inputAdvance)
            {
                SInputState.SetMouse(RealInputState.mouseState, Mouse);
                SInputState.SetKeyboard(RealInputState.keyboardState, AddedKeys, RejectedKeys);
            }
            
            // TODO: add any other logic that might lead to frame advance (stored state)
            SInputState.Active = inputAdvance;
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
            if (RealInputState.IsKeyDown(Keys.OemOpenBrackets) && RealInputState.IsKeyDown(Keys.OemCloseBrackets))
            {
                // improve this logic?
                Game1.quit = true;
                advance = true;
            }
            return advance;
        }
    }
}
