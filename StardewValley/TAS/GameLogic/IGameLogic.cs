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
        public bool Active = true;

        public bool Toggle()
        {
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

        public virtual bool ActiveUpdate(out SKeyboardState kstate, out SMouseState mstate) 
        {
            kstate = null;
            mstate = null;
            return false;
        }

        public virtual string[] HelpText()
        {
            return new string[] { string.Format(" \"{0}\": no help documentation", Name) };
        }
    }
}
