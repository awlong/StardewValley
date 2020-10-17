using Microsoft.Xna.Framework;
using StardewValley;
using System.Windows.Forms;
using TAS.GameState;

namespace TAS.Overlays
{
    public class TileGrid : IOverlay
    {
        public override string Name => "grid";
        public Color gridColor = Color.Red;
        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: draw tile grid lines", Name) };
        }

        public override void ActiveDraw(SpriteBatch spriteBatch)
        {
            int offsetX = -Game1.viewport.X % Game1.tileSize;
            int offsetY = -Game1.viewport.Y % Game1.tileSize;

            for(int k = offsetX-1; k <= Globals.ViewportWidth + Game1.tileSize; k += Game1.tileSize)
            {
                spriteBatch.Draw(SolidColor, new Rectangle(k, offsetY, 1, Globals.ViewportHeight), gridColor * 0.5f);
            }
            for (int k = offsetY - 1; k <= Globals.ViewportHeight + Game1.tileSize; k += Game1.tileSize)
            {
                spriteBatch.Draw(SolidColor, new Rectangle(offsetX, k, Globals.ViewportWidth, 1), gridColor * 0.5f);
            }
        }
    }
}
