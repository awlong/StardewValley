using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Components;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public abstract class IGameLogic : IConsoleAware
    {
        public abstract string Name { get; }
        public bool Active = true;
        public bool Toggleable = true;
        public bool Toggle()
        {
            if (Toggleable)
                Active = !Active;
            return Active;
        }

        public bool Update(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = null;
            if (Active)
                return ActiveUpdate(out kstate, out mstate);
            return false;
        }

        public abstract bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate);
    }
}
