using TAS.GameState;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class QuestionDialogueBox : IGameLogic
    {
        public override string Name => "QuestionDialogueBox";

        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
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
            // push a command as the current active subscriber and open the console
            // load the two options and ask for 1/2/3 for which menu choice they want

            return false;
        }
        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: answer question dialogue prompts", Name) };
        }
    }
}
