using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Inputs;

namespace TAS.GameLogic
{
    public abstract class IGameLogic
    {
        public abstract string Name { get; }
        public virtual bool Toggleable { get; }
        public bool Active = true;

        public bool Toggle()
        {
            if (Toggleable)
                Active = !Active;
            return Active;
        }

        public virtual bool Update(out SKeyboardState kstate, out SMouseState mstate)
        {
            kstate = null;
            mstate = null;
            return false;
        }
    }
}
