﻿using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        public LocalizedContentManager.LanguageCode DefaultLanguage = LocalizedContentManager.LanguageCode.en;
        public int DefaultSeed = 0;

        public enum Stage
        {
            Farmer,
            Farm,
            Favorite,
            Seed,
            Language,
            Prefix,
            Done
        };
        public Stage CurrentStage;
        public string FarmerName;
        public string FarmName;
        public string FavoriteThing;
        public string Prefix;
        public LocalizedContentManager.LanguageCode Language;
        public int Seed;

        public override void Run(string[] tokens)
        {
            FarmerName = "";
            FarmName = "";
            FavoriteThing = "";
            Seed = 0;
            Prefix = "";
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
                case Stage.Language:
                    return string.Format("Enter Language Code (options: [en,ja,ru,zh,pt,es,de,th,fr,ko,it,tr,hu]) (default: {0}):", DefaultLanguage);
                case Stage.Prefix:
                    return string.Format("Enter File Name (default: {0}_{1}):", FarmerName, Seed);
                case Stage.Done:
                    Unsubscribe();
                    Write("{0} | {1} | {2} | {3} | {4}", FarmerName, FarmName, FavoriteThing, Language, Seed);
                    CreateState();
                    return string.Format("New input created: {0}", Controller.State.Prefix);
                default:
                    return "shouldnt be here...";
            }
        }

        private void CreateState()
        {
            Controller.State = new SaveState(FarmerName, FarmName, FavoriteThing, Seed, Language);
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
                        Write("Seed {0} cannot be cast to integer type, please try again", input);
                    }
                    break;
                case Stage.Language:
                    if (value == "")
                    {
                        Language = DefaultLanguage;
                        CurrentStage++;
                    } 
                    else if (Enum.TryParse(value, out LocalizedContentManager.LanguageCode lang))
                    {
                        Language = lang;
                        CurrentStage++;
                    } 
                    else
                    {
                        Write("Language {0} not valid, (options: [en,ja,ru,zh,pt,es,de,th,fr,ko,it,tr,hu]) (default: {1})", value, DefaultLanguage);
                    }
                    break;
                case Stage.Prefix:
                    CurrentStage++;
                    if (value == "")
                        Prefix = string.Format("{0}_{1}", FarmerName, Seed);
                    else
                        Prefix = value;
                    break;
                default:
                    throw new Exception("shouldn't get here...");
            }
            Write(MenuLine());
        }

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: setup a new savestate file", Name),
            };
        }
    }
}
