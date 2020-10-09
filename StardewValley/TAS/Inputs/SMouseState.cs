using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Inputs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SMouseState
    {
        [JsonProperty]
        public int MouseX = 0;
        [JsonProperty]
        public int MouseY = 0;
        public int ScrollWheel = 0;
        [JsonProperty]
        public bool LeftMouseClicked = false;
        public bool MiddleMouseClicked = false;
        [JsonProperty]
        public bool RightMouseClicked = false;
        public bool XButton1Clicked = false;
        public bool XButton2Clicked = false;

        public SMouseState() { }
        public SMouseState(int mouseX, int mouseY, bool leftClick, bool rightClick)
        {
            MouseX = mouseX;
            MouseY = mouseY;
            LeftMouseClicked = leftClick;
            RightMouseClicked = rightClick;
        }
        public SMouseState(SMouseState o) : this(o.MouseX, o.MouseY, o.LeftMouseClicked, o.RightMouseClicked) { }
        public SMouseState(MouseState o) : this(o.X, o.Y, FromButtonState(o.LeftButton), FromButtonState(o.RightButton)) { }
        public SMouseState(SMouseState o, bool leftClick, bool rightClick) : this(o)
        {
            LeftMouseClicked = leftClick;
            RightMouseClicked = rightClick;
        }

        public MouseState GetMouseState()
        {
            return new MouseState(MouseX, MouseY, ScrollWheel,
                ToButtonState(LeftMouseClicked),
                ToButtonState(MiddleMouseClicked),
                ToButtonState(RightMouseClicked),
                ToButtonState(XButton1Clicked),
                ToButtonState(XButton2Clicked)
                );
        }
        private static ButtonState ToButtonState(bool state)
        {
            return state ? ButtonState.Pressed : ButtonState.Released;
        }

        private static bool FromButtonState(ButtonState state)
        {
            return state == ButtonState.Pressed;
        }
    }
}
