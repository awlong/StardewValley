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
            gameTime = StardewValley.DateTime.CurrentGameTime;
            return CanDraw;
        }

        public static void Postfix(GameTime gameTime)
        {
            if (CanDraw)
            {
                Counter++;
                StardewValley.DateTime.Update();
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

            if (!inBeginEndPair)
            {
                Game1.spriteBatch.End();
            }
        }
    }
}
