using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Components
{
    public abstract class IConsoleAware
    {
        public abstract string[] HelpText();

        public void Write(string line) { SGame.console.PushResult(line); }
        public void Write(string format, params object[] args) { Write(string.Format(format, args)); }
        public void Write(string[] lines)
        {
            foreach (var line in lines)
            {
                Write(line);
            }
        }
    }
}
