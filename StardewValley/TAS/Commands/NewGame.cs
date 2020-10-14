using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;

namespace TAS.Commands
{
    public class NewGame : ICommand
    {
        public override string Name => "newgame";

        public string DefaultFarmerName = "TAS";
        public string DefaultFarmName = "TAS Farms";
        public string DefaultFavoriteThing = "Going Fast";
        public int DefaultSeed = 0;

        public enum Stage
        {
            Farmer,
            Farm,
            Favorite,
            Seed,
            Done
        };
        public Stage CurrentStage;
        public string FarmerName;
        public string FarmName;
        public string FavoriteThing;
        public int Seed;

        public override void Run(string[] tokens)
        {
            FarmerName = "";
            FarmName = "";
            FavoriteThing = "";
            Seed = 0;

            Subscribe();
            
            CurrentStage = Stage.Farmer;
            Write("Enter Data for Field (empty -> default).");
            Write(MenuLine());

        }

        public string MenuLine()
        {
            switch (CurrentStage)
            {
                case Stage.Farmer:
                    return string.Format("Enter Farmer Name (default: {0}):", DefaultFarmerName);
                case Stage.Farm:
                    return string.Format("Enter Farm Name (default: {0}):", DefaultFarmName);
                case Stage.Favorite:
                    return string.Format("Enter Favorite Thing (default: {0}):", DefaultFavoriteThing);
                case Stage.Seed:
                    return string.Format("Enter Game Seed (default: {0}):", DefaultSeed);
                case Stage.Done:
                    Unsubscribe();
                    Write(string.Format("{0} | {1} | {2} | {3}", FarmerName, FarmName, FavoriteThing, Seed));
                    CreateState();
                    return string.Format("New input created: {0}", Controller.State.Prefix);
                default:
                    return "shouldnt be here...";
            }
        }

        private void CreateState()
        {
            Controller.State = new SaveState(FarmerName, FarmName, FavoriteThing, Seed);
            SGame.ResetGame = true;
            Controller.State.Save();

        }
        public override void ReceiveInput(string input)
        {
            string value = input.Trim();
            switch (CurrentStage)
            {
                case Stage.Farmer:
                    CurrentStage++;
                    if (value == "")
                        FarmerName = DefaultFarmerName;
                    else
                        FarmerName = value;
                    break;
                case Stage.Farm:
                    CurrentStage++;
                    if (value == "")
                        FarmName = DefaultFarmName;
                    else
                        FarmName = value;
                    break;
                case Stage.Favorite:
                    CurrentStage++;
                    if (value == "")
                        FavoriteThing = DefaultFavoriteThing;
                    else
                        FavoriteThing = value;
                    break;
                case Stage.Seed:
                    if (value == "")
                    {
                        Seed = DefaultSeed;
                        CurrentStage++;
                    }
                    else if(Int32.TryParse(value, out Seed))
                    {
                        CurrentStage++;
                    }
                    else
                    {
                        Write(string.Format("Seed {0} cannot be cast to integer type, please try again", input));
                    }
                    break;
                default:
                    throw new Exception("shouldn't get here...");
            }
            Write(MenuLine());
        }
    }
}
