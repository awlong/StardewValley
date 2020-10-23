using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;
using TAS.Utilities;
using Random = StardewValley.Random;

namespace TAS.Overlays
{
    public class MonsterDrop : IOverlay
    {
        public override string Name => "monsterdrop";
        public Color RectColor = new Color(0, 0, 0, 180);
        public Color TextColor = Color.White;
        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: display monster drops", Name) };
        }

        public override void ActiveDraw(SpriteBatch spriteBatch)
        {
            if (CurrentLocation.Active && CurrentLocation.IsMines)
            {
                foreach (NPC monster in CurrentLocation.Monsters)
                {
                    var drops = monsterDrops(monster as Monster, Game1.random.Copy(), out _);
                    var loc = monster.getStandingPosition();
                    DrawText(spriteBatch, drops, loc, TextColor, RectColor);
                    
                    // TODO: How can we estimate out for the ladder chance WHAT the random will be by drop time?
                }
            }
        }

        private static IEnumerable<string> monsterDrops(Monster monster, Random random, out bool spawnsLadder)
        {
            spawnsLadder = false;
            int x = monster.GetBoundingBox().Center.X;
            int y = monster.GetBoundingBox().Center.Y;
            MineShaft location = Game1.currentLocation as MineShaft;
            int mineLevel = CurrentLocation.MineLevel;
            Farmer who = Game1.player;
            
            List<string> items = new List<string>();
            // MineShaft::monsterDrop
            if (monster.hasSpecialItem)
            {
                items.AddRange(DropInfo.GetSpecialItemForThisMineLevel(mineLevel, x / 64, y / 64));
                random.Next(4);
            }
            else if (mineLevel > 121 && who != null && who.getFriendshipHeartLevelForNPC("Krobus") >= 10 && (int)who.houseUpgradeLevel >= 1 && !who.isMarried() && !who.isEngaged() && random.NextDouble() < 0.001)
            {
                items.Add(DropInfo.ObjectName(808));
                random.Next(4);
            }
            else
            {
                items.AddRange(baseMonsterDrops(monster, x, y, who, random));
            }

            if ((CurrentLocation.MustKillAllMonstersToAdvance() || !(random.NextDouble() < 0.15)) && (!CurrentLocation.MustKillAllMonstersToAdvance() || CurrentLocation.EnemyCount > 1))
            {
                return items;
            }

            // Why...
            Vector2 p = new Vector2(x, y) / 64f;
            p.X = (int)p.X;
            p.Y = (int)p.Y;
            Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle((int)p.X * 64, (int)p.Y * 64, 64, 64);
            string tmp = monster.Name;
            monster.Name = "ignoreMe";
            bool flag = !location.isTileOccupied(p, "ignoreMe") && location.isTileOnClearAndSolidGround(p) && !Game1.player.GetBoundingBox().Intersects(tileRect) && location.doesTileHaveProperty((int)p.X, (int)p.Y, "Type", "Back") != null && location.doesTileHaveProperty((int)p.X, (int)p.Y, "Type", "Back").Equals("Stone");
            monster.Name = tmp;
            if (flag)
            {
                spawnsLadder = true;
            }
            else if (CurrentLocation.MustKillAllMonstersToAdvance() && CurrentLocation.EnemyCount <= 1)
            {
                spawnsLadder = true;
            }
            return items;
        }

        private static IEnumerable<string> baseMonsterDrops(Monster monster, int x, int y, Farmer who, Random random)
        {
            List<string> items = new List<string>();
            if (monster is GreenSlime slime)
                items.AddRange(GreenSlime_ExtraDropItems(slime, random));

            // TODO: wearing ring stuff
            // actual items
            foreach (var obj in monster.objectsToDrop)
            {
                items.Add(DropInfo.ObjectName(obj));
                if (obj < 0)
                    random.Next(1, 4);
            }
            // TODO: Wearing ring stuff
            // TODO: Magnifying glass
            return items;
        }

        private static IEnumerable<string> GreenSlime_ExtraDropItems(GreenSlime slime, Random gameRandom)
        {
            Color color = slime.color;
            List<string> extra = new List<string>();
            if (color.R < 80 && color.G < 80 && color.B < 80)
            {
                extra.Add(DropInfo.ObjectName(382)); gameRandom.Next();
                Random random = new Random((int)slime.Position.X * 777 + (int)slime.Position.Y * 77 + (int)Game1.stats.DaysPlayed);
                if (random.NextDouble() < 0.05)
                {
                    extra.Add(DropInfo.ObjectName(553)); gameRandom.Next();
                }
                if (random.NextDouble() < 0.05)
                {
                    extra.Add(DropInfo.ObjectName(539)); gameRandom.Next();
                }
            }
            else if (color.R > 200 && color.G > 180 && color.B < 50)
            {
                extra.Add(DropInfo.ObjectName(384) + "x2"); gameRandom.Next();
            }
            else if (color.R > 220 && color.G > 90 && color.G < 150 && color.B < 50)
            {
                extra.Add(DropInfo.ObjectName(378) + "x2"); gameRandom.Next();
            }
            else if (color.R > 230 && color.G > 230 && color.B > 230)
            {
                extra.Add(DropInfo.ObjectName(380)); gameRandom.Next();
                if ((int)color.R % 2 == 0 && (int)color.G % 2 == 0 && (int)color.B % 2 == 0)
                {
                    extra.Add(DropInfo.ObjectName(72)); gameRandom.Next();
                }
            }
            else if (color.R > 150 && color.G > 150 && color.B > 150)
            {
                extra.Add(DropInfo.ObjectName(390) + "x2"); gameRandom.Next();
            }
            else if (color.R > 150 && color.B > 180 && color.G < 50 && (int)slime.specialNumber % (slime.firstGeneration ? 4 : 2) == 0)
            {
                extra.Add(DropInfo.ObjectName(386) + "x2"); gameRandom.Next();
            }
            if (Game1.MasterPlayer.mailReceived.Contains("slimeHutchBuilt") && (int)slime.specialNumber == 1)
            {
                string name = slime.Name;
                if (!(name == "Green Slime"))
                {
                    if (name == "Frost Jelly")
                    {
                        extra.Add(DropInfo.ObjectName(413)); gameRandom.Next();
                    }
                }
                else
                {
                    extra.Add(DropInfo.ObjectName(680)); gameRandom.Next();
                }
            }
            return extra;
        }
    }
}
