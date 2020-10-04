using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Reflection;

namespace TAS.Wrappers
{
    public class DrawWrapper
    {
        public static bool CanDraw;
        public static int Counter;

        public static void Reset()
        {
            CanDraw = false;
            Counter = 0;
        }

        public static bool Prefix(ref GameTime gameTime)
        {
            // draw after update
            CanDraw = (Counter + 1) == UpdateWrapper.Counter;
            return CanDraw;
        }

        public static void Postfix(GameTime gameTime)
        {
            if (CanDraw)
            {
                // TODO: do stuff
                Counter++;
            }
            else
            {
                RedrawFrame();
                InvokeBase(gameTime);
            }
            CanDraw = false;
        }

        public static void RedrawFrame()
        {
            // TODO: write RenderScreenBuffer code
        }

        public static void InvokeBase(GameTime gameTime)
        {
            var method = typeof(Game).GetMethod("Draw", BindingFlags.NonPublic | BindingFlags.Instance);
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
