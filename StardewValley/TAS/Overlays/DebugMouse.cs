using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Overlays
{
    public class DebugMouse : IOverlay
    {
        public override string Name => "mouse";

        public Color MouseColor = Color.Black;
        public DebugMouse()
        {
            Active = true;
        }

        public override void DrawImpl(SpriteBatch spriteBatch)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 coords = new Vector2(
                (float)mouseState.X * (1f / Game1.options.zoomLevel),
                (float)mouseState.Y * (1f / Game1.options.zoomLevel)
            );
            spriteBatch.Draw(Game1.mouseCursors, coords,
                Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.mouseCursor, 16, 16),
                MouseColor * Game1.mouseCursorTransparency, 0f, Vector2.Zero, 4f + Game1.dialogueButtonScale / 150f,
                SpriteEffects.None, 1f);
        }
    }

}
