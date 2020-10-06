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
                gameTime = gameTime; // TODO: modify later to overriden datetime
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
            else
            {
                // ensure the base Game.Update is called to prevent freezing
                InvokeBase(gameTime);
            }
            CanUpdate = false;
        }

        public static void InvokeBase(GameTime gameTime)
        {
            var method = typeof(Game).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
            var funcPtr = method.MethodHandle.GetFunctionPointer();
            if (Game1.game1 != null)
            {
                // get the actual base function
                var func = (Action<GameTime>)Activator.CreateInstance(typeof(Action<GameTime>), Game1.game1, funcPtr);
                func(gameTime);
            }
        }
    }
}
