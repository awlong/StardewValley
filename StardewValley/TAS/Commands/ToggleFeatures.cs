using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.GameLogic;
using TAS.Overlays;

namespace TAS.Commands
{
    public class ToggleLogic : ICommand
    {
        public override string Name => "logic";
        private string[] validStates = new string[] { "off", "false", "f", "on", "true", "t" };
        public override void Run(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                foreach(IGameLogic logic in Controller.GameLogics.Values)
                {
                    Write("{0}: {1}", logic.Name, logic.Active);
                }
                return;
            }
            if (tokens.Length == 1)
            {
                Write(HelpText());
                return;
            }
            if (tokens.Length == 2)
            {
                if (!validStates.Contains(tokens[1].ToLower()))
                {
                    Write("Invalid token: second passed token must be on/true/t or off/false/f");
                    return;
                }

                if (tokens[0].ToLower() == "all")
                {
                    foreach (var name in Controller.GameLogics.Keys)
                    {
                        Toggle(name, tokens[1]);
                    }
                }
                else
                {
                    Toggle(tokens[0], tokens[1]);
                }
            }
        }

        private void Toggle(string name, string state)
        {
            if (Controller.GameLogics.ContainsKey(name))
            {
                switch (state.ToLower())
                {
                    case "on":
                    case "true":
                    case "t":
                        Controller.GameLogics[name].Active = true;
                        break;
                    case "off":
                    case "false":
                    case "f":
                        Controller.GameLogics[name].Active = false;
                        break;
                    default:
                        throw new ArgumentException("state should have been handled before here");
                }
                Write("{0} -> {1}", name, state);
            }
        }
        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("See current state: {0}", Name),
                string.Format("Toggle usage: {0} <name> on|off", Name),
                string.Format("Toggle all usage: {0} all on|off", Name),
            };
        }
    }

    public class ToggleOverlay : ICommand
    {
        public override string Name => "overlay";
        private string[] validStates = new string[] { "off", "false", "f", "on", "true", "t" };
        public override void Run(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                Write("Overlays (call \"overlay <name> on/off\" to change, or \"overlay all on/off\"):");
                foreach (IOverlay overlay in Controller.Overlays.Values)
                {
                    Write("{0}: {1}", overlay.Name, overlay.Active);
                }
                return;
            }
            if (tokens.Length == 1)
            {
                Write(HelpText());
                return;
            }
            if (tokens.Length == 2)
            {
                if (!validStates.Contains(tokens[1].ToLower()))
                {
                    Write("Invalid token: second passed token must be on/true/t or off/false/f");
                    return;
                }

                if (tokens[0].ToLower() == "all")
                {
                    foreach(var name in Controller.Overlays.Keys)
                    {
                        Toggle(name, tokens[1]);
                    }
                }
                else
                {
                    Toggle(tokens[0], tokens[1]);
                }
            }
        }

        private void Toggle(string name, string state)
        {
            if (Controller.Overlays.ContainsKey(name))
            {
                switch (state.ToLower())
                {
                    case "on":
                    case "true":
                    case "t":
                        Controller.Overlays[name].Active = true;
                        break;
                    case "off":
                    case "false":
                    case "f":
                        Controller.Overlays[name].Active = false;
                        break;
                    default:
                        throw new ArgumentException("state should have been handled before here");
                }
                Write("{0} -> {1}", name, state);
            }
        }

        public override string[] HelpText()
        {
            return new string[]
            {
                string.Format("See current state: {0}", Name),
                string.Format("Toggle usage: {0} <name> on|off", Name),
                string.Format("Toggle all usage: {0} all on|off", Name),
            };
        }
    }
}
