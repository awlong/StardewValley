using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TAS.Commands
{
    public abstract class ICommand
    {
        public abstract string Name { get; }
        public abstract void Run(string[] tokens);

        public virtual void ReceiveInput(string input) { }

        public void Subscribe() { SGame.console.ActiveSubscribers.Push(Name); }
        public void Unsubscribe() 
        { 
            if (SGame.console.ActiveSubscribers.Peek() != Name)
                throw new Exception("Tried to pop from non-controlling command!");
            SGame.console.ActiveSubscribers.Pop(); 
        }

        public void Write(string line) { SGame.console.PushResult(line); }
        public void Write(string format, params object[] args) { Write(string.Format(format, args)); }
        public void Write(string[] lines)
        {
            foreach(var line in lines)
            {
                Write(line);
            }
        }
        public virtual string[] ParseToken(string token) { return null; }

        public virtual string[] HelpText()
        {
            return new string[] { string.Format(" \"{0}\": no help documentation", Name) };
        }
    }
}
