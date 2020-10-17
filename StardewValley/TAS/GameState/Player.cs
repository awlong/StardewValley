using Netcode;
using StardewValley;
using StardewValley.Network;
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

        public static NetStringDictionary<Friendship, NetRef<Friendship>> Friendships { get { return Game1.player?.friendshipData;} }
        public static string CurrentAnimationStartBehavior
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
                        else if (!CanMove)
                        {
                            behavior = "frozen";
                        }
                    }
                }
                return behavior;
            }
        }
        public static string LastAnimationEndBehavior
        {
            get
            {
                string behavior = null;
                if (Game1.player != null && Game1.player.FarmerSprite != null)
                {
                    int animIndex = Game1.player.FarmerSprite.currentAnimationIndex;
                    if (animIndex < Game1.player.FarmerSprite.CurrentAnimation.Count && animIndex > 0)
                    {
                        if (Game1.player.FarmerSprite.CurrentAnimation[animIndex-1].frameEndBehavior != null)
                        {
                            behavior = Game1.player.FarmerSprite.CurrentAnimation[animIndex-1].frameEndBehavior.Method.Name;
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
