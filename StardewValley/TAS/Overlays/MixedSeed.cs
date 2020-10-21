using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;
using TAS.Inputs;
using TAS.Utilities;

namespace TAS.Overlays
{
    public class MixedSeed : IOverlay
    {
        public override string Name => "mixedseed";

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: display mixed seed that will be planted", Name) };
        }

        public override void ActiveDraw(SpriteBatch spriteBatch)
        {
            if (CurrentLocation.Active && Game1.currentLocation is Farm farm)
            {
                int tileX = (RealInputState.mouseState.X + Game1.viewport.X) / Game1.tileSize;
                int tileY = (RealInputState.mouseState.Y + Game1.viewport.Y) / Game1.tileSize;
                Vector2 tile = new Vector2(tileX, tileY);

                if (farm.terrainFeatures.ContainsKey(tile) && farm.terrainFeatures[tile] is HoeDirt dirt && dirt.crop == null)
                {
                    if (Game1.player.CurrentItem is StardewValley.Object obj && obj.parentSheetIndex == 770)
                    {
                        int cropIndex = SeasonInfo.GetRandomLowGradeCropForThisSeason();
                        DrawObjectSpriteAtTile(spriteBatch, tile, cropIndex);
                    }
                }
            }
        }
    }
}
