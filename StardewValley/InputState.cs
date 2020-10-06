using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TAS.Wrappers;

namespace StardewValley
{
	public class InputState
	{
		public virtual void Update()
		{
		}

		public virtual KeyboardState GetKeyboardState()
		{
			// Adding an postfix to update if there is input to be injected
			KeyboardState keyboardState = Keyboard.GetState();
			InputStateWrapper.Keyboard.Postfix(ref keyboardState);
			return keyboardState;
		}

		public virtual GamePadState GetGamePadState()
		{
			if (Game1.options.gamepadMode == Options.GamepadModes.ForceOff)
			{
				return default(GamePadState);
			}
			return GamePad.GetState(PlayerIndex.One);
		}

		public virtual MouseState GetMouseState()
		{
			// Adding an postfix to update if there is input to be injected
			MouseState mouseState = Mouse.GetState();
			InputStateWrapper.Mouse.Postfix(ref mouseState);
			return mouseState;
		}
	}
}
