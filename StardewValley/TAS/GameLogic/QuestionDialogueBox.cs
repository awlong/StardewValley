using TAS.GameState;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class QuestionDialogueBox : IGameLogic
    {
        public override string Name => "QuestionDialogueBox";

        public override bool Update(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = null;

            if (!CurrentMenu.Active || !CurrentMenu.IsDialogue || !CurrentMenu.IsQuestion)
                return false;

            string question = CurrentMenu.CurrentString;
            if (question.Equals("Go to sleep for the night?"))
            {
                kstate = new SKeyboardState("Y");
                return true;
            }

            return false;
        }
    }
}
