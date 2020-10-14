using Newtonsoft.Json;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TAS
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SaveState
    {
        [JsonProperty]
        public string FarmerName = "abc";
        [JsonProperty]
        public string FarmName = "abc";
        [JsonProperty]
        public string FavoriteThing = "abc";
        [JsonProperty]
        public string Prefix = "tmp";
        [JsonProperty]
        public int Seed = 0;
        [JsonProperty]
        public StateList FrameStates = new StateList();

        public SaveState()
        {
            if (Game1.player != null)
            {
                FarmerName = Game1.player.Name;
                FarmName = Game1.player.farmName;
                FavoriteThing = Game1.player.favoriteThing;
            }
        }

        public SaveState(string farmerName, string farmName, string favoriteThing, int seed)
        {
            FarmerName = farmerName;
            FarmName = farmName;
            FavoriteThing = favoriteThing;
            Seed = seed;
            Prefix = string.Format("{0}_{1}", farmerName, seed);
        }
        public SaveState(StateList states) : base()
        {
            FrameStates.AddRange(states);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2} Frames:{3}", FarmerName, FarmName, FavoriteThing, Count);
        }

        public string FilePath
        {
            get
            {
                return Path.Combine(Constants.SaveStatePath, Prefix + ".json");
            }
        }

        [JsonProperty]
        public int Count
        {
            get
            {
                return FrameStates.Count;
            }
        }

        public static string PathFromPrefix(string prefix)
        {
            return Path.Combine(Constants.SaveStatePath, prefix + ".json");
        }

        public void Save()
        {
            using (StreamWriter file = File.CreateText(FilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, this);
            }
        }

        public static SaveState Load(string prefix = "tmp")
        {
            SaveState state = null;
            string filePath = SaveState.PathFromPrefix(prefix);
            if (File.Exists(filePath))
            {
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    // TODO: any safety rails for overwriting current State?
                    state = (SaveState)serializer.Deserialize(file, typeof(SaveState));
                    Debug.WriteLine(state.ToString());
                }
            }
            return state;
        }

        public static void ChangeSaveStatePrefix(string filePath, string newPrefix)
        {
            SaveState state = null;
            if (File.Exists(filePath))
            {
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    // TODO: any safety rails for overwriting current State?
                    state = (SaveState)serializer.Deserialize(file, typeof(SaveState));
                }
                state.Prefix = newPrefix;
                state.Save();
            }
        }

    }
}
