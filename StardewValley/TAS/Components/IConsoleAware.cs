using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Components
{
    public abstract class IConsoleAware
    {
        public abstract string[] HelpText();

        public void Write(string line)
        {
            foreach (var field in line.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                SGame.console.PushResult(field);
            }
        }
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
