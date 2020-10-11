using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Reflection;

namespace TAS.Wrappers
{
    public class UpdateWrapper
    {
        public static bool CanUpdate;
        public static int Counter;

        public static void Reset()
        {
            CanUpdate = false;
            Counter = 0;
        }

        public static bool Prefix(ref GameTime gameTime)
        {
            // frame lock the updates with draws
            if (DrawWrapper.Counter != UpdateWrapper.Counter)
            {
                CanUpdate = false;
            }
            else
            {
                CanUpdate = Controller.Update();
                gameTime = StardewValley.DateTime.CurrentGameTime;
            }
            return CanUpdate;
        }

        public static void Postfix(GameTime gameTime)
        {
            if (CanUpdate)
            {
                // we updated the frame, make sure we can draw on next call
                Counter++;
            }
            CanUpdate = false;
        }
    }
}
