using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Overlays
{
    public class MouseData : IOverlay
    {
        public override string Name => "mousedata";

        public Color RectColor = new Color(0, 0, 0, 180);
        public Color TextColor = Color.White;
        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: draw data for coord", Name) };
        }
        public override void ActiveDraw(SpriteBatch spriteBatch)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 coords = new Vector2(
                (float)mouseState.X * (1f / Game1.options.zoomLevel),
                (float)mouseState.Y * (1f / Game1.options.zoomLevel)
            );
            
            int mouseTileX = (mouseState.X + Game1.viewport.X) / Game1.tileSize;
            int mouseTileY = (mouseState.Y + Game1.viewport.Y) / Game1.tileSize;

            // open the door for extra data to be written down the line
            List<string> data = new List<string>();
            data.Add(string.Format("({0},{1})", mouseTileX, mouseTileY));
            DrawText(spriteBatch, data, coords, TextColor, RectColor);
        }
    }
}
