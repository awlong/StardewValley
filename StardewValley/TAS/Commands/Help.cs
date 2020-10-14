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
                Write(HelpText()[0]);
            }
            else
            {
                foreach (var token in tokens)
                {
                    foreach (var line in ParseToken(token))
                    {
                        Write(line);
                    }
                }
            }
        }

        public override string[] ParseToken(string token)
        {
            if (SGame.console.Commands.ContainsKey(token))
            {
                return SGame.console.Commands[token].HelpText();
            }
            return new string[] { string.Format("command {0} not found", token) };
        }

        public override string[] HelpText()
        {
            return new string[] { string.Format("usage: \"{0} <func>\" to get function details", Name) };
        }
    }
}
