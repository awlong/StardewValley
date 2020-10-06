using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Inputs
{
    public class SKeyboardState : HashSet<Keys>
    {
        public SKeyboardState() : base() { }
        public SKeyboardState(IEnumerable<Keys> keys) : base()
        {
            foreach (var key in keys)
            {
                Add(key);
            }
        }

        public SKeyboardState(KeyboardState state) : this(state.GetPressedKeys()) { }

        public SKeyboardState(string key) : base()
        {
            if (key != "")
            {
                Add(key);
            }
        }

        public SKeyboardState(string[] keys) : base()
        {
            foreach (var key in keys)
            {
                if (key == "")
                    continue;
                Add(key);
            }
        }

        public bool Add(string key)
        {
            return Add((Keys)Enum.Parse(typeof(Keys), key));
        }

        public KeyboardState GetKeyboardState()
        {
            return new KeyboardState(this.ToArray());
        }
    }
}
