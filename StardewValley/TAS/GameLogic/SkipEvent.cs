using TAS.GameState;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class SkipEvent : IGameLogic
    {
        public override string Name => "SkipEvent";
        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);
            if (!CurrentLocation.Active || !CurrentEvent.Active)
                return false;

            if (CurrentEvent.Skippable)
            {
                if (CurrentEvent.LastCommand == "skippable")
                    kstate = new SKeyboardState("F");
                return true;
            }
            if (CurrentEvent.CurrentCommand.Contains("pause"))
                return true;

            return false;
        }

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: auto skip events", Name) };
        }
    }
}
