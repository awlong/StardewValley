using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Commands
{
    public class LoadSaveState : ICommand
    {
        public override string Name => "load";

        public override void Run(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                Write("\tNo input file given");
                return;
            }
            string prefix = tokens[0].Replace(".json","").Trim();
            SaveState state = SaveState.Load(prefix);
            if(state == null)
            {
                Write("\tInput file \"{0}\" not found", tokens[0]);
                return;
            }
            Controller.State = state;
            SGame.ResetGame = true;
            Write("Loaded {0} ({1} frames)", tokens[0], Controller.State.Count);
        }
    }

    public class FastLoadSaveState : ICommand
    {
        public override string Name => "fload";

        public override void Run(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                Write("\tNo input file given");
                return;
            }
            string prefix = tokens[0].Replace(".json", "").Trim();
            SaveState state = SaveState.Load(prefix);
            if (state == null)
            {
                Write("\tInput file \"{0}\" not found", tokens[0]);
                return;
            }
            Controller.State = state;
            SGame.ResetGame = true;
            SGame.FastAdvance = true;
            Write("Loaded {0} ({1} frames)", tokens[0], Controller.State.Count);
        }
    }

    public class SaveSaveState : ICommand
    {
        public override string Name => "save";
        public override void Run(string[] tokens)
        {
            Controller.State.Save();
            Write("Wrote save to {0}", Controller.State.Prefix);
        }
    }

    public class SaveStateInfo : ICommand
    {
        public override string Name => "stateinfo";

        public override void Run(string[] tokens)
        {
            string[] fields = Controller.State.ToString().Split('|');
            foreach (var field in fields)
            {
                foreach (var line in field.Split(new char[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries))
                {
                    Write(line);
                }
            }
        }
    }
}
