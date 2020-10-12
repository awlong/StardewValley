using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.GameState
{
    public class Player
    {
        public static bool Active { get { return Game1.player != null; } }
        public static bool CanMove { get { return Game1.player.CanMove; } }
        public static bool UsingTool { get { return Game1.player.UsingTool; } }
        public static Tool CurrentTool { get { return Game1.player?.CurrentTool; } }
        public static string CurrentAnimationBehavior
        {
            get
            {
                string behavior = null;
                if (Game1.player != null && Game1.player.FarmerSprite != null)
                {
                    int animIndex = Game1.player.FarmerSprite.currentAnimationIndex;
                    if (animIndex < Game1.player.FarmerSprite.CurrentAnimation.Count)
                    {
                        if (Game1.player.FarmerSprite.CurrentAnimation[animIndex].frameStartBehavior != null)
                        {
                            behavior = Game1.player.FarmerSprite.CurrentAnimation[animIndex].frameStartBehavior.Method.Name;
                        }
                        else if (Game1.player.FarmerSprite.CurrentAnimation[animIndex].frameEndBehavior != null)
                        {
                            behavior = Game1.player.FarmerSprite.CurrentAnimation[animIndex].frameEndBehavior.Method.Name;
                        }
                        else if (!CanMove)
                        {
                            behavior = "frozen";
                        }
                    }
                }
                return behavior;
            }
        }
    }
}
