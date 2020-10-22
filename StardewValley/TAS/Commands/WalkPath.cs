using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Commands
{
    public class WalkPath : ICommand
    {
        public override string Name => "walkpath";

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: run the current path", Name) };
        }

        public override void Run(string[] tokens)
        {
            if (SGame.pathFinder.hasPath)
            {
                GameLogic.WalkPath.DoWalk = true;
                Write(string.Format("\tWalking to point ({0},{1})", SGame.pathFinder.PeekBack().X, SGame.pathFinder.PeekBack().Y));
            }
            else
            {
                Write("no path set, please generate a path first: {0}", new GeneratePath().Name);
            }
        }
    }
}
