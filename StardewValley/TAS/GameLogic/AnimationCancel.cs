using StardewValley.Tools;
using TAS.Inputs;
using TAS.GameState;
using Microsoft.Xna.Framework.Input;

namespace TAS.GameLogic
{
    public class AnimationCancel : IGameLogic
    {
        public override string Name => "AnimationCancel";
        public override bool Update(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = new SMouseState(Controller.LastFrameMouse(), false, false);
            if (!Player.Active || !Player.UsingTool)
                return false;
            if (Player.CurrentTool is MeleeWeapon || Player.CurrentTool is FishingRod)
                return false;

            // we're now in a tool use scenario, check for the right frame
            kstate = new SKeyboardState();
            string behavior = Player.LastAnimationEndBehavior;
            if (behavior == null || !behavior.Equals("useTool"))
                behavior = Player.CurrentAnimationStartBehavior;
            if (behavior != null && behavior.Equals("useTool"))
            {
                kstate.Add(Keys.RightShift);
                kstate.Add(Keys.Delete);
                kstate.Add(Keys.R);
            }
            return true;
        }
    }
}
