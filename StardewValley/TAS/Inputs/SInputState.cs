using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Inputs
{
    public class SInputState
    {
        public static bool Active;

        public static SMouseState mState = new SMouseState();
        public static SKeyboardState kState = new SKeyboardState();

        public static void Reset()
        {
            Active = false;
            mState = new SMouseState();
            kState = new SKeyboardState();
        }

        public static KeyboardState GetKeyboard()
        {
            return kState.GetKeyboardState();
        }

        public static void AddKey(Keys key)
        {
            kState.Add(key);
        }

        public static void AddKeys(Keys[] keys)
        {
            foreach(var key in keys)
            {
                AddKey(key);
            }
        }

        public static void RemoveKey(Keys key)
        {
            kState.Remove(key);
        }

        public static void RemoveKeys(Keys[] keys)
        {
            foreach(var key in keys)
            {
                RemoveKey(key);
            }
        }

        public static void ClearKeys()
        {
            kState.Clear();
        }

        public static MouseState GetMouse()
        {
            return mState.GetMouseState();
        }

        public static void MoveMouse(int x, int y)
        {
            mState.MouseX = x;
            mState.MouseY = y;
        }

        public static void SetLeftClick(bool click)
        {
            mState.LeftMouseClicked = click;
        }

        public static void SetRightClick(bool click)
        {
            mState.RightMouseClicked = click;
        }

        public static void SetMouse(MouseState state)
        {
            //TODO: handle case where there are automation logic changes for mouse
            MoveMouse(state.X, state.Y);
            SetLeftClick(state.LeftButton == ButtonState.Pressed);
            SetRightClick(state.RightButton == ButtonState.Pressed);
        }

        public static void SetKeyboard(KeyboardState state)
        {
            //TODO: handle case where there are rejected/extra keys from automation logic
            ClearKeys();
            AddKeys(state.GetPressedKeys());
        }
    }
}
