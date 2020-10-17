using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Commands
{
    public class AdvanceFrames : ICommand
    {
        public override string Name => "advance";
        private string[] validKeys = { "s", "sec", "second" };
        public override void Run(string[] tokens)
        {
            if (tokens.Length > 1)
            {
                Write(HelpText());
                return;
            }
            
            if (tokens.Length == 0)
            {
                GameLogic.AdvanceFrame.SetAdvance(1);
            }
            else
            {
                if (validKeys.Contains(tokens[0].ToLower()))
                {
                    GameLogic.AdvanceFrame.SetAdvance(StardewValley.DateTime.FramesToNextSecond);
                }
                else if(Int32.TryParse(tokens[0], out int frames))
                {
                    GameLogic.AdvanceFrame.SetAdvance(frames);
                }
                else
                {
                    Write("invalid input: {0} could not be parsed to int or \"s\"");
                }
            }

        }

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: advance a number of frames", Name),
                string.Format("advance 1 frame: \"advance\""),
                string.Format("advance N frame: \"advance N\""),
                string.Format("advance next second: \"advance s\"")
            };
        }
    }


}
