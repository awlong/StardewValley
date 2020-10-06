using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Inputs;

namespace TAS.Wrappers
{
    public class InputStateWrapper
    {
        public class Keyboard
        {
            public static void Postfix(ref KeyboardState __result)
            {
                if (SInputState.Active)
                {
                    __result = SInputState.GetKeyboard();
                }
            }
        }

        public class Mouse
        {
            public static void Postfix(ref MouseState __result)
            {
                if (SInputState.Active)
                {
                    __result = SInputState.GetMouse();
                }
            }
        }
    }
}
