using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;
using xTile.Dimensions;

namespace TAS.Commands
{
    public class GetFriendship : ICommand
    {
        public override string Name => "friendship";
        public static GetFriendship _instance;
        public GetFriendship()
        {
            _instance = this;
        }
        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: get friendship info", Name),
                string.Format("all friendships: \"{0}\"", Name),
                string.Format("single friendship: \"{0} <name>\"", Name)
            };
        }

        public override void Run(string[] tokens)
        {
            if (tokens.Length > 1)
            {
                Write(HelpText());
            }
            else if(tokens.Length == 1)
            {
                if (Player.Friendships.ContainsKey(tokens[0]))
                {
                    Write("\t{0}: {1}/{2}", tokens[0], Player.Friendships[tokens[0]].Points, Player.Friendships[tokens[0]].Points / 250);
                }
                else
                {
                    Write("invalid name: {0} is not a npc with friendship", tokens[0]);
                }
            }
            else
            {
                foreach (string npc in Player.Friendships.Keys)
                {
                    Write("\t{0}: {1}/{2}", npc, Player.Friendships[npc].Points, Player.Friendships[npc].Points / 250);
                }
            }
        }
    }

    public class GetPlayer : ICommand
    {
        public override string Name => "player";

        public override string[] HelpText()
        {
            return new string[] 
            {
                string.Format("{0}: get player data info", Name),
                string.Format("all player data: \"{0}\"", Name),
                string.Format("player position: \"{0} pos\"", Name),
                string.Format("current luck: \"{0} luck\"", Name),
                string.Format("current steps: \"{0} steps\"", Name),
                string.Format("current hp: \"{0} hp\"", Name),
                string.Format("current energy: \"{0} energy\"", Name),
                string.Format("current xp: \"{0} xp\"", Name),
                string.Format("friendships: \"{0} friendships\"", Name),
            };
        }

        public override void Run(string[] tokens)
        {
            if (tokens.Length > 1)
            {
                Write(HelpText());
            }
            if (tokens.Length == 0)
            {
                tokens = new string[] { "pos", "luck", "steps", "hp", "energy", "xp", "friendships" };
            }
            foreach (var token in tokens)
            {
                switch (token)
                {
                    case "pos":
                    case "position":
                        Write("\tPosition: {0}, {1}", Game1.player.getStandingX() / Game1.tileSize, Game1.player.getStandingY() / Game1.tileSize);
                        break;
                    case "luck":
                        Write("\tDaily Luck: {0}", Game1.player.DailyLuck);
                        break;
                    case "steps":
                        Write("\tSteps Taken: {0}", Game1.stats.StepsTaken);
                        break;
                    case "energy":
                        Write("\tEnergy: {0}", Game1.player.Stamina);
                        break;
                    case "hp":
                        Write("\tHealth: {0}", Game1.player.health);
                        break;
                    case "xp":
                        Write("\tExperience:");
                        Write("\t\tFarming: {0}", Game1.player.experiencePoints[0]);
                        Write("\t\tFishing: {0}", Game1.player.experiencePoints[1]);
                        Write("\t\tForage : {0}", Game1.player.experiencePoints[2]);
                        Write("\t\tMining : {0}", Game1.player.experiencePoints[3]);
                        Write("\t\tCombat : {0}", Game1.player.experiencePoints[4]);
                        break;
                    case "friend":
                    case "friendship":
                    case "friendships":
                        Write("\tFriendships:");
                        GetFriendship._instance.Run(new string[] { "all" });
                        break;
                    default:
                        Write("invalid token: {0} not in list of options", token);
                        return;
                }
            }
        }
    }
}
