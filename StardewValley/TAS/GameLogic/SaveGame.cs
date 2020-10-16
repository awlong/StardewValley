using TAS.GameState;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public class SaveGame : IGameLogic
    {
        public override string Name => "SaveGame";
        public override bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);
            if (CurrentMenu.IsSaveGame)
            {
                return !CurrentMenu.CanQuit;
            }
            return false;
        }

        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: advance frame through night save", Name) };
        }
    }
}
