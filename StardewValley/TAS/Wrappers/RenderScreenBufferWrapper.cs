using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StardewValley;

namespace TAS.Wrappers
{
    public class RenderScreenBufferWrapper
    {
        public static bool Prefix(ref RenderTarget2D screen)
        {
            return false;
        }

        private static Color color => Color.CornflowerBlue;
        public static void Base(RenderTarget2D screen)
        {
            if (screen != null)
            {
                Game1.GraphicsDevice.SetRenderTarget(null);
                Game1.GraphicsDevice.Clear(color);

                // TODO: should this change when we override SpriteBatch?
                bool inBeginEndPair = (bool)Reflector.GetValue(Game1.spriteBatch, "inBeginEndPair");
                if (!inBeginEndPair)
                {
                    Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone);
                }

                Game1.spriteBatch.Draw(screen, Vector2.Zero, screen.Bounds, Color.White, 0f, Vector2.Zero, Game1.options.zoomLevel, SpriteEffects.None, 1f);

                if (!inBeginEndPair)
                {
                    Game1.spriteBatch.End();
                }
            }
        }

        public static void Postfix(RenderTarget2D screen)
        {
            if(Game1.game1.takingMapScreenshot)
            {
                Game1.GraphicsDevice.SetRenderTarget(null);
            }
            else
            {
                Base(screen);
            }
        }
    }
}
