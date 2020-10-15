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
            Write("Loaded {0} ({1} frames", tokens[0], Controller.State.Count);
        }
    }
}
