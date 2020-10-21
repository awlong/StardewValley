using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Locations;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;
using Object = StardewValley.Object;

namespace TAS.Overlays
{
    public class ObjectTiles : IOverlay
    {
        public override string Name => "objecttiles";

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: draw object/clump outlines", Name) };
        }
        
        public static Color ObjectColor = Color.Aquamarine;
        public static Color TerrainFeatureColor = Color.Fuchsia;
        public static Color LargeTerrainFeatureColor = Color.Brown;
        public static Color ResourceClumpColor = Color.BlanchedAlmond;

        public override void ActiveDraw(SpriteBatch spriteBatch)
        {
            if (CurrentLocation.Active)
            {
                foreach (KeyValuePair<Vector2, Object> current in Game1.currentLocation.Objects.Pairs)
                {
                    DrawTileOutline(spriteBatch, current.Key, ObjectColor);
                }
                foreach (KeyValuePair<Vector2, TerrainFeature> current in Game1.currentLocation.terrainFeatures.Pairs)
                {
                    DrawTileOutline(spriteBatch, current.Key, TerrainFeatureColor);
                }
                foreach (LargeTerrainFeature current in Game1.currentLocation.largeTerrainFeatures)
                {
                    if (current is Bush bush)
                    {
                        Vector2 scale;
                        switch ((int)bush.size)
                        {
                            case 0:
                            case 3:
                                scale = new Vector2(1, 1);
                                break;
                            case 1:
                                scale = new Vector2(2, 1);
                                break;
                            case 2:
                                scale = new Vector2(3, 1);
                                break;
                            default:
                                scale = new Vector2(1, 1);
                                break;
                        }
                        DrawTileOutline(spriteBatch, current.tilePosition.Value, LargeTerrainFeatureColor, scale);
                    }
                }

                if (Game1.currentLocation is MineShaft mineShaft)
                {
                    foreach (ResourceClump current in mineShaft.resourceClumps)
                    {
                        DrawTileOutline(spriteBatch, current.tile.Value, ResourceClumpColor, 2f);
                    }
                }
                if (Game1.currentLocation is Farm farm)
                {
                    foreach (ResourceClump current in farm.resourceClumps)
                    {
                        DrawTileOutline(spriteBatch, current.tile.Value, ResourceClumpColor, 2f);
                    }
                }
            }
        }
    }
}