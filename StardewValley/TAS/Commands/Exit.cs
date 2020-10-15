using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Commands
{
    public class Exit : ICommand
    {
        public override string Name => "exit";

        public override void Run(string[] tokens)
        {
            Program.gamePtr.Exit();
        }
    }
}
