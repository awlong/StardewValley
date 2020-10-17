using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StardewValley;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Inputs;

namespace TAS
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FrameState
    {
        public Random random;
        [JsonProperty (ItemConverterType=typeof(StringEnumConverter))]
        public SKeyboardState keyboardState;
        [JsonProperty]
        public SMouseState mouseState;
        [JsonProperty]
        public string comments;
        [JsonProperty]
        public ulong Frame;

        public FrameState()
        {
            random = Game1.random.Copy();
            keyboardState = new SKeyboardState();
            mouseState = new SMouseState();
            comments = "";
            Frame = DateTime.CurrentFrame;
        }

        public FrameState(int seed, string comm="")
        {
            random = new Random(seed);
            comments = comm;
            Frame = DateTime.CurrentFrame;
        }

        public FrameState(FrameState o)
        {
            random = o.random.Copy();
            keyboardState = new SKeyboardState(o.keyboardState);
            mouseState = new SMouseState(o.mouseState);
            comments = o.comments;
            Frame = o.Frame;
        }

        public FrameState(KeyboardState kstate, MouseState mstate, string comm= "")
        {
            random = Game1.random.Copy();
            keyboardState = new SKeyboardState(kstate);
            keyboardState.IntersectWith(ValidKeys);
            mouseState = new SMouseState(mstate);
            comments = comm;
            Frame = DateTime.CurrentFrame;
        }

        public void toStates(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = new SKeyboardState(keyboardState);
            mstate = new SMouseState(mouseState);
        }

        public void toStates(out KeyboardState kstate, out MouseState mstate)
        {
            kstate = keyboardState.GetKeyboardState();
            mstate = mouseState.GetMouseState();
        }



        public static Keys[] ValidKeys =
        {
            // Inventory
            Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.OemMinus, Keys.OemPlus,
            // Movement
            Keys.W, Keys.A, Keys.S, Keys.D,
            // Actions
            Keys.C, Keys.F, Keys.Y, Keys.X,
            // Menus
            Keys.Escape, Keys.E, Keys.I, Keys.M, Keys.J,
            // Escape Keys
            Keys.RightShift, Keys.R, Keys.Delete,
            // Misc
            Keys.LeftShift, Keys.LeftControl
        };
    }
}
