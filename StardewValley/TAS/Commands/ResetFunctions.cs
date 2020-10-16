using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TAS.Commands
{
    public class GameReset : ICommand
    {
        public override string Name => "reset";

        public override void Run(string[] tokens)
        {
            if (tokens.Length > 1)
            {
                Write(HelpText());
                return;
            }
            int resetTo = -1;
            if (tokens.Length == 1)
            {
                if (!Int32.TryParse(tokens[0], out resetTo))
                {
                    Write("invalid frame: {0} could not be parsed to integer", tokens[0]);
                    return;
                }
            }
            Controller.State.Reset(resetTo);
            SGame.ResetGame = true;
            Write(string.Format("reset to frame {0}", Controller.State.Count));
        }

        public override string[] HelpText()
        {
            return new string[] 
            { 
                string.Format("{0}: reset game and advance", Name),
                string.Format("reset to end: \"reset\""),
                string.Format("reset to specific frame: \"reset <frame#>\""),
                string.Format("negative frames offset from end (\"reset -1\" is equivalent to \"reset\")")
            };
        }
    }
    public class GameResetFast : ICommand
    {
        public override string Name => "freset";

        public override void Run(string[] tokens)
        {
            if (tokens.Length > 1)
            {
                Write(HelpText());
                return;
            }
            int resetTo = -1;
            if (tokens.Length == 1)
            {
                if (!Int32.TryParse(tokens[0], out resetTo))
                {
                    Write("invalid frame: {0} could not be parsed to integer", tokens[0]);
                    return;
                }
            }
            Controller.State.Reset(resetTo);
            SGame.ResetGame = true;
            SGame.FastAdvance = true;
            Write(string.Format("fast reset to frame {0}", Controller.State.Count));
        }

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: reset game and advance (fast)", Name),
                string.Format("reset to end: \"reset\""),
                string.Format("reset to specific frame: \"reset <frame#>\""),
                string.Format("negative frames offset from end (\"reset -1\" is equivalent to \"reset\")")
            };
        }
    }
}
