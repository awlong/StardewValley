using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class AdvanceFrame : IGameLogic
    {
        public override string Name => "AdvanceFrame";
        private static AdvanceFrame _instance;
        public static int NumFrames = 0;

        public AdvanceFrame()
        {
            _instance = this;
            Toggleable = false;
        }
        public static void SetAdvance(int numFrames = 1)
        {
            if (numFrames > 0)
            {
                NumFrames = numFrames;
                _instance.Write("advancing {0} frames", NumFrames);
            }
        }
        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);
            if (NumFrames-- > 0)
                return true;
            return false;
        }

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("{0}: advance forward the set number of frames", Name)
            };
        }
    }
}
