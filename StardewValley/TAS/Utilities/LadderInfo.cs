using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;
using Object = StardewValley.Object;
using Random = StardewValley.Random;

namespace TAS.Utilities
{
    public class LadderInfo
    {
        public static int StonesRemaining(MineShaft mine, Vector2 loc)
        {
            int stonesLeftOnThisLevel = CurrentLocation.StonesLeftOnThisLevel();
            if (CurrentLocation.LadderHasSpawned() || (stonesLeftOnThisLevel == 0))
            {
                return -1;
            }

            double rockChance = (mine.EnemyCount == 0 ? 0.06 : 0.02) + (double)Game1.player.LuckLevel / 100.0 + Game1.player.DailyLuck / 5.0;
            for (int i = 0; i < stonesLeftOnThisLevel; i++)
            {
                Random random = new Random((int)loc.X * 1000 + (int)loc.Y + mine.mineLevel + (int)Game1.uniqueIDForThisGame / 2);
                random.NextDouble();
                if (random.NextDouble() < rockChance + 1.0 / (double)Math.Max(1, stonesLeftOnThisLevel - i))
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool HasLadder(out Vector2 location)
        {
            location = Vector2.Zero;
            if (CurrentLocation.LadderHasSpawned())
            {
                // have to find it...
                if (Game1.currentLocation is MineShaft mine)
                {
                    xTile.Dimensions.Size mapDims = mine.map.Layers[0].LayerSize;
                    xTile.Layers.Layer tileLayer = mine.map.GetLayer("Buildings");
                    if (tileLayer != null)
                    {
                        for (int i = 0; i < mapDims.Width; i++)
                        {
                            for (int j = 0; j < mapDims.Height; j++)
                            {
                                if (tileLayer.Tiles[i, j] == null)
                                    continue;
                                if (tileLayer.Tiles[i, j].TileIndex == 174 || tileLayer.Tiles[i, j].TileIndex == 173)
                                {
                                    location = new Vector2(i, j);
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

    }
}
