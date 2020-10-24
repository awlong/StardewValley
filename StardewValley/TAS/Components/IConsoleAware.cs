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

        public void RunAlias(string alias)
        {
            string command = GetAlias(alias);
            if (command != "")
                SGame.console.RunCommand(command);
        }

        public IEnumerable<string> GetAllAliases()
        {
            return SGame.console.Aliases.Keys;
        }
        public string GetAlias(string alias)
        {
            if (SGame.console.Aliases.ContainsKey(alias))
                return SGame.console.Aliases[alias];
            return "";
        }
        public void SetAlias(string alias, string command)
        {
            if (SGame.console.Aliases.ContainsKey(alias))
                SGame.console.Aliases[alias] = command;
            else
                SGame.console.Aliases.Add(alias, command);
        }
    }
}
