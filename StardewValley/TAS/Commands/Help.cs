using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace TAS.Commands
{
    public class Help : ICommand
    {
        public override string Name => "help";

        public override void Run(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                Write(HelpText());
            }
            else
            {
                foreach (var token in tokens)
                {
                    Write(ParseToken(token));
                }
            }
        }

        public override string[] ParseToken(string token)
        {
            List<string> lines = new List<string>();
            if (SGame.console.Commands.ContainsKey(token))
            {
                lines.Add(string.Format("Command: {0}", token));
                lines.AddRange(SGame.console.Commands[token].HelpText());
            }
            if (Controller.Overlays.ContainsKey(token))
            {
                lines.Add(string.Format("Overlay: {0}", token));
                lines.AddRange(Controller.Overlays[token].HelpText());
            }
            if (Controller.GameLogics.ContainsKey(token))
            {
                lines.Add(string.Format("GameLogic: {0}", token));
                lines.AddRange(Controller.GameLogics[token].HelpText());
            }

            if (lines.Count > 0)
                return lines.ToArray();

            return new string[] { string.Format("command {0} not found", token) };
        }

        public override string[] HelpText()
        {
            return new string[] { string.Format("usage: \"{0} <func>\" to get function details", Name) };
        }
    }

    public class ListCommands : ICommand
    {
        public override string Name => "list";
        public string[] topLevelKeys = { "overlay", "logic", "commands" };
        public override void Run(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                Write(HelpText());
                return;
            }

            switch(tokens[0].ToLower())
            {
                case "overlay":
                case "o":
                    ListKeys("Overlays", Controller.Overlays.Keys);
                    break;
                case "logic":
                case "l":
                    ListKeys("GameLogics", Controller.GameLogics.Keys);
                    break;
                case "commands":
                case "comm":
                case "c":
                    ListKeys("Commands", SGame.console.Commands.Keys);
                    break;
                default:
                    Write(HelpText());
                    break;
            }
        }

        private void ListKeys(string header, IEnumerable<string> keys)
        {
            Write("Available {0} options (call help <func> for more info)", header);
            int idx = 0;
            foreach(var key in keys)
            {
                Write(string.Format("\t{0:0000}: {1}", idx++, key));
            }
        }

        public override string[] HelpText()
        {
            List<string> text = new List<string>();
            text.Add(string.Format("usage: \"{0} <key>\" to list available items", Name));
            text.Add("Top level keys:");
            int idx = 0;
            foreach (var key in topLevelKeys)
            {
                text.Add(string.Format("\t{0:0000}: {1}", idx, key));
                idx++;
            }
            return text.ToArray();
        }
    }
}
