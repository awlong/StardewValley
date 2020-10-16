using TAS.GameState;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class ScreenFade : IGameLogic
    {
        public override string Name => "ScreenFade";

        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);

            if (!CurrentLocation.Active || CurrentEvent.Active || CurrentMenu.Active)
                return false;

            if (Globals.GlobalFade)
                return true;
            if (Globals.FadeIn && Globals.FadeToBlackAlpha < 1f && Globals.FadeToBlackAlpha != 0)
                return true;
            if (Globals.FadeToBlack && Globals.FadeToBlackAlpha > 0f)
                return true;

            return false;
        }

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: advance frame through screen fades", Name) };
        }
    }
}
