using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameState;

namespace TAS.Commands
{
    public class GetForage : ICommand
    {
        public override string Name => "forage";

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: get forage details", Name),
                string.Format("for current loc: \"{0}\"", Name),
                string.Format("for specific loc: \"{0} locName\"", Name),
                string.Format("for all locs: \"{0} all\"", Name)
            };
        }

        public override void Run(string[] tokens)
        {
            if (tokens.Length > 1)
            {
                Write(HelpText());
            }
            else if (tokens.Length == 1)
            {
                if (tokens[0].ToLower() == "all")
                {
                    foreach (var val in CurrentLocation.AllForage)
                    {
                        WriteForLocation(val.Key, val.Value);
                    }
                }
                else
                {
                    GameLocation location = Game1.getLocationFromName(tokens[0]);
                    if (location == null)
                    {
                        Write("invalid location name: location {0} not found", tokens[0]);
                    }
                    else
                    {
                        WriteForLocation(location.name, CurrentLocation.LocationForage(location));
                    }
                }
            }
            else
            {
                if (CurrentLocation.Active)
                {
                    WriteForLocation(CurrentLocation.Name, CurrentLocation.Forage);
                }
            }
        }

        private void WriteForLocation(string name, IEnumerable<KeyValuePair<Vector2, StardewValley.Object>> pairs)
        {
            if (pairs.Count() > 0)
            {
                Write("{0}:", name);
                int i = 0;
                foreach (var pair in pairs)
                {
                    if (pair.Value.ParentSheetIndex == 590)
                        Write("\t{0:000}: ({1},{2}) -> {3} -> {4}", i, pair.Key.X, pair.Key.Y, pair.Value.Name, ArtifactSpot.DigUp(pair.Key, name));
                    else
                        Write("\t{0:000}: ({1},{2}) -> {3}", i, pair.Key.X, pair.Key.Y, pair.Value.Name);
                    i++;
                }
            }
        }
    }

    public class GetTrashCans : ICommand
    {
        private Dictionary<string, Tuple<Vector2, int>> TrashCans;
        public GetTrashCans()
        {
            TrashCans = new Dictionary<string, Tuple<Vector2, int>>();
            TrashCans.Add("Jodi  ", new Tuple<Vector2, int>(new Vector2(13,86), 0));
            TrashCans.Add("Emily ", new Tuple<Vector2, int>(new Vector2(19,89), 1));
            TrashCans.Add("Lewis ", new Tuple<Vector2, int>(new Vector2(56,85), 2));
            TrashCans.Add("Museum", new Tuple<Vector2, int>(new Vector2(108,91), 3));
            TrashCans.Add("Clint ", new Tuple<Vector2, int>(new Vector2(97,80), 4));
            TrashCans.Add("Saloon", new Tuple<Vector2, int>(new Vector2(47,70), 5));
            TrashCans.Add("Alex  ", new Tuple<Vector2, int>(new Vector2(52,63), 6));
            TrashCans.Add("Joja  ", new Tuple<Vector2, int>(new Vector2(110,56), 7));
        }

        public override string Name => "trashcans";

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: get trash can drops", Name)
            };
        }

        public override void Run(string[] tokens)
        {
            if (!CurrentLocation.Active)
                return;

            foreach(var can in TrashCans)
            {
                string trashItem = CurrentLocation.CheckTrash(can.Value.Item1, can.Value.Item2);
                if (trashItem != "")
                    Write("\t{0}: {1}", can.Key, trashItem);
            }
        }
    }
}
