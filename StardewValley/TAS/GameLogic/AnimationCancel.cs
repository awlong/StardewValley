using TAS.Inputs;
using TAS.Utilities;

namespace TAS.GameLogic
{
    public class AnimationCancel : IGameLogic
    {
        public override string Name => "AnimationCancel";

        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
        {
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);
            return MovementInfo.ShouldAnimCancel(out kstate);
        }

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: auto animation cancel", Name) };
        }
    }
}
