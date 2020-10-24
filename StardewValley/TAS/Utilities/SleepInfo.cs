using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Network;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Object = StardewValley.Object;
using Random = StardewValley.Random;

namespace TAS.Utilities
{
    public class SleepInfo
    {
        private static Random random;
        private static int dishOfTheDay;
        private static double dailyLuck;
        private static string whichFriend;
        private static bool receiveGift;
        private static bool lightningTriggered;
        private static int weatherForTomorrow;

        private static NetVector2Dictionary<TerrainFeature, Netcode.NetRef<TerrainFeature>> Farm_terrainFeatures;

        private static int last_dayOfMonth;
        private static uint last_stats_DaysPlayed;
        private static uint last_stats_StepsTaken;
        private static int last_timeOfDay;
        private static int last_Farm_terrainFeatures_Count;

        public static int DishOfTheDay
        {
            get
            {
                if (NeedsUpdate())
                    RecomputeStats();
                return dishOfTheDay;
            }
        }

        public static double DailyLuck
        {
            get
            {
                if (NeedsUpdate())
                    RecomputeStats();
                return dailyLuck;
            }
        }

        public static string WhichFriend
        {
            get
            {
                if (NeedsUpdate())
                    RecomputeStats();
                return whichFriend;
            }
        }

        public static bool ReceiveGift
        {
            get
            {
                if (NeedsUpdate())
                    RecomputeStats();
                return receiveGift;
            }
        }

        public static bool LightningTriggered
        {
            get
            {
                if (NeedsUpdate())
                    RecomputeStats();
                return lightningTriggered;
            }
        }

        public static int WeatherForTomorrow
        {
            get
            {
                if (NeedsUpdate())
                    RecomputeStats();
                return weatherForTomorrow;
            }
        }


        public static bool NeedsUpdate()
        {
            if (last_dayOfMonth == Game1.dayOfMonth &&
                last_stats_DaysPlayed == Game1.stats.DaysPlayed &&
                last_stats_StepsTaken == Game1.stats.StepsTaken &&
                last_timeOfDay == Game1.timeOfDay &&
                last_Farm_terrainFeatures_Count == Game1.getLocationFromName("Farm").terrainFeatures.Count())
            {
                return false;
            }
            return true;
        }

        public static void RecomputeStats()
        {
            Farm_terrainFeatures = new NetVector2Dictionary<TerrainFeature, Netcode.NetRef<TerrainFeature>>(Game1.getLocationFromName("Farm").terrainFeatures.Pairs);

            last_dayOfMonth = Game1.dayOfMonth;
            last_stats_DaysPlayed = Game1.stats.DaysPlayed;
            last_stats_StepsTaken = Game1.stats.StepsTaken;
            last_timeOfDay = Game1.timeOfDay;
            last_Farm_terrainFeatures_Count = Farm_terrainFeatures.Count();

            int dayOfMonth = Game1.dayOfMonth;
            uint stats_DaysPlayed = Game1.stats.DaysPlayed;
            int seed = (int)Game1.uniqueIDForThisGame / 100 + (int)(stats_DaysPlayed * 10) + 1 + (int)Game1.stats.StepsTaken;

            random = new Random(seed);
            for (int index = 0; index < dayOfMonth; ++index)
                random.Next();

            /*** Dish Of the Day ***/
            dishOfTheDay = random.Next(194, 240);
            while (Utility.getForbiddenDishesOfTheDay().Contains(dishOfTheDay))
                dishOfTheDay = random.Next(194, 240);

            int dishOfTheDayQty = random.Next(1, 4 + ((random.NextDouble() < 0.08) ? 10 : 0));
            random.NextDouble(); // Object::Constructor() flipped.Value

            // Minutes Elapsed Block
            foreach (GameLocation location in Game1.locations)
            {
                for (int n = location.objects.Count() - 1; n >= 0; n--)
                {
                }
            }
            foreach (Building building in Game1.getFarm().buildings)
            {
                if (building.indoors.Value != null)
                {
                }
            }
            if (Game1.isLightning)
                lightningTriggered = OvernightLightning();
            else
                lightningTriggered = false;

            /*** Friendship Gift ***/
            if (Game1.player.friendshipData.Count() > 0)
            {
                whichFriend = Game1.player.friendshipData.Keys.ElementAt(random.Next(Game1.player.friendshipData.Keys.Count()));
                receiveGift = random.NextDouble() < (double)(Game1.player.friendshipData[whichFriend].Points / 250) * 0.1 &&
                    (Game1.player.spouse == null || !Game1.player.spouse.Equals(whichFriend)) &&
                    Game1.content.Load<Dictionary<string, string>>("Data\\mail").ContainsKey(whichFriend);
            }

            random.Next(); // scarecrow society in Farmer.dayupdate();

            /*** Daily Luck ***/
            dailyLuck = Math.Min(0.10000000149011612, (double)random.Next(-100, 101) / 1000.0);

            dayOfMonth++;
            stats_DaysPlayed++;
            string currentSeason = Game1.currentSeason;
            int year = Game1.year;

            if (dayOfMonth == 29)
            {
                // Game1.newSeason()
                switch (Game1.currentSeason)
                {
                    case "spring":
                        currentSeason = "summer";
                        break;
                    case "summer":
                        currentSeason = "fall";
                        break;
                    case "fall":
                        currentSeason = "winter";
                        break;
                    case "winter":
                        currentSeason = "spring";
                        year++;
                        break;
                }
                dayOfMonth = 1;
            }

            /*** Weather Block ***/
            WorldDate date = new WorldDate(year, currentSeason, dayOfMonth);
            weatherForTomorrow = Game1.getWeatherModificationsForDate(date, Game1.weatherForTomorrow);
            bool isRaining = false;
            bool isLightning = false;
            bool isSnowing = false;
            bool wasRainingYesterday = (Game1.isRaining || Game1.isLightning);
            if (weatherForTomorrow == 1 || weatherForTomorrow == 3)
                isRaining = true;
            if (weatherForTomorrow == 3)
                isLightning = true;
            if (weatherForTomorrow == 0 || weatherForTomorrow == 2 || weatherForTomorrow == 4 || weatherForTomorrow == 5 || weatherForTomorrow == 6)
            {
                isRaining = false;
                isLightning = false;
                isSnowing = weatherForTomorrow == 5;
            }

            if (weatherForTomorrow == 2)
                PopulateDebrisWeatherArray();

            double chanceToRainTomorrow;
            if (currentSeason.Equals("summer"))
            {
                chanceToRainTomorrow = ((dayOfMonth > 1) ? (0.12 + (double)((float)dayOfMonth * 0.003f)) : 0.0);
            }
            else if (currentSeason.Equals("winter"))
            {
                chanceToRainTomorrow = 0.63;
            }
            else
            {
                chanceToRainTomorrow = 0.183;
            }

            weatherForTomorrow = 0;
            if (random.NextDouble() < chanceToRainTomorrow)
            {
                weatherForTomorrow = 1;
                if ((currentSeason.Equals("summer") && random.NextDouble() < 0.85) || (!currentSeason.Equals("winter") && random.NextDouble() < 0.25 && dayOfMonth > 2 && stats_DaysPlayed > 27))
                {
                    weatherForTomorrow = 3;
                }
                if (currentSeason.Equals("winter"))
                {
                    weatherForTomorrow = 5;
                }
            }
            else if (stats_DaysPlayed > 2 && ((currentSeason.Equals("spring") && random.NextDouble() < 0.2) || (currentSeason.Equals("fall") && random.NextDouble() < 0.6)))
            {
                weatherForTomorrow = 2;
            }
            else
            {
                weatherForTomorrow = 0;
            }
            if (Utility.isFestivalDay(dayOfMonth + 1, currentSeason))
            {
                weatherForTomorrow = 4;
            }
            if (stats_DaysPlayed == 2)
            {
                weatherForTomorrow = 1;
            }
        }

        private static bool OvernightLightning()
        {
            int numberOfLoops = (2400 - Game1.timeOfDay) / 100;
            bool triggered = false;
            for (int i = 0; i < numberOfLoops; i++)
            {
                triggered |= performLightningUpdate();
            }
            return triggered;
        }
        private static bool performLightningUpdate()
        {
            bool triggered = false;
            Random currRandom = new Random((int)Game1.uniqueIDForThisGame + (int)Game1.stats.DaysPlayed + Game1.timeOfDay);
            if (currRandom.NextDouble() < 0.125 + Game1.player.DailyLuck + Game1.player.luckLevel / 100f)
            {
                Farm farm = Game1.getLocationFromName("Farm") as Farm;
                List<Vector2> list = new List<Vector2>();
                foreach (KeyValuePair<Vector2, Object> pair in farm.objects.Pairs)
                {
                    if ((bool)pair.Value.bigCraftable && pair.Value.ParentSheetIndex == 9)
                    {
                        list.Add(pair.Key);
                    }
                }
                if (list.Count > 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 vector = list.ElementAt(currRandom.Next(list.Count));
                        if (farm.objects[vector].heldObject.Value == null)
                        {
                            random.NextDouble(); // Object::ctor() flipped.Value set
                            return true;
                        }
                    }
                }
                if (currRandom.NextDouble() < 0.25 - Game1.player.team.AverageDailyLuck() - Game1.player.team.AverageLuckLevel() / 100.0)
                {
                    triggered = true;
                    try
                    {
                        KeyValuePair<Vector2, TerrainFeature> keyValuePair = Farm_terrainFeatures.Pairs.ElementAt(currRandom.Next(Farm_terrainFeatures.Count()));
                        if (!(keyValuePair.Value is FruitTree))
                        {
                            if (TerrainFeature_PerformToolAction(keyValuePair.Value, null, 50, keyValuePair.Key, farm))
                            {
                                Farm_terrainFeatures.Remove(keyValuePair.Key);
                            }
                        }
                        else if (keyValuePair.Value is FruitTree fruitTree)
                        {
                            FruitTree_Shake(fruitTree, keyValuePair.Key, true, farm);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return triggered;
        }

        private static bool TerrainFeature_PerformToolAction(TerrainFeature terrainFeature, Tool t, int damage, Vector2 tileLocation, GameLocation location)
        {
            if (terrainFeature is Grass grass)
            {
                random.NextDouble(); // Grass Shake (Grass.cs:228)
                for (int i = 0; i < 4; i++) // multiplayer.BroadcastSprites (Grass.cs:261)
                    random.NextDouble();
            }
            return false;
        }
        private static void FruitTree_Shake(FruitTree tree, Vector2 tileLocation, bool doEvenIfStillShaking, Farm farm)
        {
            // TODO
        }

        private static void PopulateDebrisWeatherArray()
        {
            int debrisToMake = random.Next(16, 64);
            for (int i = 0; i < debrisToMake; i++)
            {
                // Game1::populateDebrisWeatherArray initialize the variables for constructor
                float randomVector_X = (float)random.Next(0, Game1.viewport.Width);
                float randomVector_Y = (float)random.Next(0, Game1.viewport.Height);
                float rotationVelocity = random.Next(15) / 500f;
                float dx = random.Next(-10, 0) / 50f;
                float dy = random.Next(10) / 50f;
                // WeatherDebris::constructor() animationIntervalOffset
                int tmp = random.Next();
            }
        }
    }
}
