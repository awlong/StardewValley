using System;
using StardewValley;
using StardewValley.Tools;
using TAS.Inputs;
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
            if (Game1.player == null || !Game1.player.usingTool)
                return false;
            if (Game1.player.CurrentTool is MeleeWeapon || Game1.player.CurrentTool is FishingRod)
                return false;
            
            // we're now in a tool use scenario, check for the right frame
            kstate = new SKeyboardState();
            AnimatedSprite.endOfAnimationBehavior behavior = GetAnimationBehavior();
            if (behavior != null && behavior.Method.Name.Equals("useTool"))
            {
                kstate.Add(Keys.RightShift);
                kstate.Add(Keys.Delete);
                kstate.Add(Keys.R);
            }
            return true;
        }

        private AnimatedSprite.endOfAnimationBehavior GetAnimationBehavior()
        {
            AnimatedSprite.endOfAnimationBehavior behavior = null;
            if (Game1.player.FarmerSprite != null)
            {
                int animIndex = Game1.player.FarmerSprite.currentAnimationIndex;
                if (Game1.player.FarmerSprite.CurrentAnimation[animIndex].frameStartBehavior != null)
                {
                    behavior = Game1.player.FarmerSprite.CurrentAnimation[animIndex].frameStartBehavior;
                }
                else if (animIndex > 0 && Game1.player.FarmerSprite.CurrentAnimation[animIndex - 1].frameEndBehavior != null)
                {
                    behavior = Game1.player.FarmerSprite.CurrentAnimation[animIndex - 1].frameEndBehavior;
                }
            }
            return behavior;
        }
    }
}
