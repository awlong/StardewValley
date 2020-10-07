using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            gameTime = gameTime; // TODO: modify later to overriden datetime
            return CanDraw;
        }
        public static bool ImplPrefix(GameTime gameTime, ref RenderTarget2D screen)
        {
            screen = Reflector.GetValue<Game1, RenderTarget2D>(Game1.game1, "screen");
            return true;
        }
        public static void Postfix(GameTime gameTime)
        {
            if (CanDraw)
            {
                // TODO: update post-frame stuff (DateTime)
                Counter++;
            }
            else
            {
                RedrawFrame(gameTime);
            }

            Controller.Draw();
            CanDraw = false;
        }

        public static void RedrawFrame(GameTime gameTime)
        {
            // TODO: should this change when we override SpriteBatch?
            bool inBeginEndPair = (bool)Reflector.GetValue(Game1.spriteBatch, "inBeginEndPair");
            if (!inBeginEndPair)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            }

            // draw the stored frame back to the screen
            RenderTarget2D target_screen = Reflector.GetValue<Game1, RenderTarget2D>(Game1.game1, "screen");
            RenderScreenBufferWrapper.Base(target_screen);

            // Run the base Game.Draw function so game doesn't hang
            InvokeBase(gameTime);

            if (!inBeginEndPair)
            {
                Game1.spriteBatch.End();
            }
            
        }

        public static void InvokeBase(GameTime gameTime)
        {
            // TODO: Should this use the reflector logic?
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
