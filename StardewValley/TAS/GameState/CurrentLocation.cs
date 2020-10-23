using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = StardewValley.Random;

namespace TAS.GameState
{
    public class CurrentLocation
    {
        public static bool Active { get { return Game1.currentLocation != null; } }
        public static string Name { get { return Game1.currentLocation?.Name; } }

        public static IEnumerable<NPC> Characters
        {
            get
            {
                return Game1.currentLocation?.characters.Where((n) => (!(n is Monster)));
            }
        }

        public static IEnumerable<NPC> Monsters
        {
            get
            {
                return (Game1.currentLocation?.characters.Where((n) => (n is Monster)));
            }
        }

        public static IEnumerable<KeyValuePair<Vector2, StardewValley.Object>> Forage
        {
            get
            {
                return LocationForage(Game1.currentLocation);
            }
        }

        public static Dictionary<string, IEnumerable<KeyValuePair<Vector2, StardewValley.Object>>> AllForage
        {
            get
            {
                Dictionary<string, IEnumerable<KeyValuePair<Vector2, StardewValley.Object>>> forage = new Dictionary<string, IEnumerable<KeyValuePair<Vector2, StardewValley.Object>>>();
                foreach(GameLocation location in Game1.locations)
                {
                    if (location.Name == "Desert" && !Game1.player.hasOrWillReceiveMail("ccVault"))
                        continue;
                    forage.Add(location.Name, LocationForage(location));
                }
                return forage;
            }
        }
        public static IEnumerable<KeyValuePair<Vector2, StardewValley.Object>> LocationForage(GameLocation location)
        {
            if (location == null)
                return null;
            return location.Objects.Pairs.Where((pair) =>
            {
                return IsForage(location, pair.Value.Category, pair.Value.ParentSheetIndex);
            });
        }
        private static bool IsForage(GameLocation location, int category, int parentSheetIndex)
        {
            if(category != -79 && category != -81 && category != -80 && category != -75 && !(location is Beach))
            {
                return (int)parentSheetIndex == 430 || parentSheetIndex == 590;
            }
            return true;
        }

        public static string CheckTrash(Vector2 tileLocation, int num)
        {
            Random random = new Random((int)Game1.uniqueIDForThisGame / 2 + (int)Game1.stats.DaysPlayed + 777 + num * 77);
            int num2 = random.Next(0, 100);
            for (int i = 0; i < num2; i++)
            {
                random.NextDouble();
            }
            num2 = random.Next(0, 100);
            for (int j = 0; j < num2; j++)
            {
                random.NextDouble();
            }

            int num3 = Utility.getSeasonNumber(Game1.currentSeason) * 17;
            bool flag = (Game1.stats.getStat("trashCansChecked") + 1) > 20 && random.NextDouble() < 0.01;
            bool flag2 = (Game1.stats.getStat("trashCansChecked") + 1) > 20 && random.NextDouble() < 0.002;
            if (flag2)
            {
                return "Hat::TrashCanLid";
            }
            if (!flag && !(random.NextDouble() < 0.2 + Game1.player.DailyLuck))
            {
                return "";
            }
            int parentSheetIndex = 168;
            switch (random.Next(10))
            {
                case 0:
                    parentSheetIndex = 168;
                    break;
                case 1:
                    parentSheetIndex = 167;
                    break;
                case 2:
                    parentSheetIndex = 170;
                    break;
                case 3:
                    parentSheetIndex = 171;
                    break;
                case 4:
                    parentSheetIndex = 172;
                    break;
                case 5:
                    parentSheetIndex = 216;
                    break;
                case 6:
                    parentSheetIndex = Utility.getRandomItemFromSeason(Game1.currentSeason, (int)tileLocation.X * 653 + (int)tileLocation.Y * 777, forQuest: false);
                    break;
                case 7:
                    parentSheetIndex = 403;
                    break;
                case 8:
                    parentSheetIndex = 309 + random.Next(3);
                    break;
                case 9:
                    parentSheetIndex = 153;
                    break;
            }
            if (num == 3 && random.NextDouble() < 0.2 + Game1.player.DailyLuck)
            {
                parentSheetIndex = 535;
                if (random.NextDouble() < 0.05)
                {
                    parentSheetIndex = 749;
                }
            }
            if (num == 4 && random.NextDouble() < 0.2 + Game1.player.DailyLuck)
            {
                parentSheetIndex = 378 + random.Next(3) * 2;
                random.Next(1, 5);
            }
            if (num == 5 && random.NextDouble() < 0.2 + Game1.player.DailyLuck && Game1.dishOfTheDay != null)
            {
                parentSheetIndex = (((int)Game1.dishOfTheDay.parentSheetIndex != 217) ? ((int)Game1.dishOfTheDay.parentSheetIndex) : 216);
            }
            if (num == 6 && random.NextDouble() < 0.2 + Game1.player.DailyLuck)
            {
                parentSheetIndex = 223;
            }
            if (num == 7 && random.NextDouble() < 0.2)
            {
                if (!Utility.HasAnyPlayerSeenEvent(191393))
                {
                    parentSheetIndex = 167;
                }
                if (Utility.doesMasterPlayerHaveMailReceivedButNotMailForTomorrow("ccMovieTheater") && !Utility.doesMasterPlayerHaveMailReceivedButNotMailForTomorrow("ccMovieTheaterJoja"))
                {
                    parentSheetIndex = ((!(random.NextDouble() < 0.25)) ? 270 : 809);
                }
            }
            if (Game1.objectInformation.ContainsKey(parentSheetIndex))
            {
                return Game1.objectInformation[parentSheetIndex];
            }
            return "unknown item";
        }

        public static bool IsMines { get { return Game1.currentLocation is MineShaft; } }
        public static int MineLevel { get { return (Game1.currentLocation as MineShaft).mineLevel; } }
        public static int StonesLeftOnThisLevel()
        {
            if (Game1.currentLocation is MineShaft mine)
            {
                return Reflector.GetValue<MineShaft, int>(mine, "stonesLeftOnThisLevel");
            }
            return 0;
        }
        public static bool LadderHasSpawned()
        {
            if (Game1.currentLocation is MineShaft mine)
            {
                if (mine.mineLevel % 10 == 0 || mine.mineLevel % 40 == 12)
                    return true;
                return Reflector.GetValue<MineShaft, bool>(mine, "ladderHasSpawned");
            }
            return false;
        }
        public static int EnemyCount
        {
            get
            {
                if (Game1.currentLocation is MineShaft mine)
                    return mine.EnemyCount;
                return 0;
            }
        }
        public static bool MustKillAllMonstersToAdvance()
        {
            if (Game1.currentLocation is MineShaft mine)
            {
                return mine.mustKillAllMonstersToAdvance();
            }
            return false;
        }
    }
}
