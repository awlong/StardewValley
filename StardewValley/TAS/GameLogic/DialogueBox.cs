using TAS.GameState;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class DialogueBox : IGameLogic
    {
        public override string Name => "DialogueBox";

        public override bool Update(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);

            if (!CurrentMenu.Active || !CurrentMenu.IsDialogue)
                return false;

            // transitioning on/off screen
            if (CurrentMenu.Transitioning)
                return true;

            // force the characters on the screen
            if (CurrentMenu.CharacterIndexInDialogue == 0)
            {
                mstate = new SMouseState((int)Globals.ViewportCenter.X,
                    (int)Globals.ViewportCenter.Y, true, false);
                return true;
            }

            // waiting after characters have loaded
            if (!CurrentMenu.IsQuestion && CurrentMenu.SafetyTimer > 0)
                return true;

            return false;
        }
    }
}
