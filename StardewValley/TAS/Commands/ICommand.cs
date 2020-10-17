using System;
using TAS.Components;

namespace TAS.Commands
{
    public abstract class ICommand : IConsoleAware
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

        public virtual string[] ParseToken(string token) { return null; }
    }
}
