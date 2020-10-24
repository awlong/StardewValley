using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Commands
{
    public class SaveEngine : ICommand
    {
        public override string Name => "saveengine";

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: save current engine state", Name) };
        }

        public override void Run(string[] tokens)
        {
            Program.gamePtr.SaveEngineState();
        }
    }
}
