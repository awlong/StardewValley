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

        public abstract string[] HelpText();

        public void Write(string line) { SGame.console.PushResult(line); }
        public void Write(string format, params object[] args) { Write(string.Format(format, args)); }
        public void Write(string[] lines)
        {
            foreach (var line in lines)
            {
                Write(line);
            }
        }
    }
}
