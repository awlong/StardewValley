using System.Collections.Generic;
using TAS.GameState;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class AdvanceFrozen : IGameLogic
    {
        public override string Name => "AdvanceFrozen";

        private List<string> ValidStrings = new List<string>{ "showHoldingItem", "showReceiveNewItemMessage", "doSleepEmote", "frozen"};
        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);

            // only want to advance if can't move in a non-tool scenario
            if (!Player.Active || Player.CanMove || Player.UsingTool)
                return false;
            // we can't move cause other stuff is going on
            if (CurrentMenu.Active || CurrentEvent.Active)
                return false;

            string behavior = Player.LastAnimationEndBehavior; 
            if (behavior == null)
                behavior = Player.CurrentAnimationStartBehavior;
            if (behavior != null)
            {
                if (ValidStrings.Contains(behavior))
                    return true;
            }
            return false;
        }
        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: advance when player is mid-animation state", Name) };
        }
    }
}
