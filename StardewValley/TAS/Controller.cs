using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TAS.GameLogic;
using TAS.Inputs;
using TAS.Overlays;
using TAS.Wrappers;
using DateTime = StardewValley.DateTime;

namespace TAS
{
    public class Controller
    {
        public static Dictionary<string, IOverlay> Overlays;
        public static Dictionary<string, IGameLogic> GameLogics;

        public static SaveState State;

        private static SMouseState RealMouse;
        private static HashSet<Keys> RejectedRealKeys;
        private static HashSet<Keys> AddedRealKeys;
        private static SMouseState LogicMouse;
        private static HashSet<Keys> RejectedLogicKeys;
        private static HashSet<Keys> AddedLogicKeys;

        static Controller()
        {

            Overlays = new Dictionary<string, IOverlay>();
            foreach (var v in Reflector.GetTypesInNamespace(ResetWrapper.ExecutingAssembly, "TAS.Overlays"))
            {
                if (v.IsAbstract || v.BaseType != typeof(IOverlay))
                    continue;
                IOverlay overlay = (IOverlay)Activator.CreateInstance(v);
                Overlays.Add(overlay.Name, overlay);
                Debug.WriteLine("Overlay \"{0}\" added to overlays list", overlay.Name);
            }

            GameLogics = new Dictionary<string, IGameLogic>();
            foreach (var v in Reflector.GetTypesInNamespace(ResetWrapper.ExecutingAssembly, "TAS.GameLogic"))
            {
                if (v.IsAbstract || v.BaseType != typeof(IGameLogic))
                    continue;
                IGameLogic logic = (IGameLogic)Activator.CreateInstance(v);
                GameLogics.Add(logic.Name, logic);
                Debug.WriteLine("GameLogic \"{0}\" added to logic list", logic.Name);
            }

            State = new SaveState();
            
            RealMouse = null;
            RejectedRealKeys = new HashSet<Keys>();
            AddedRealKeys = new HashSet<Keys>();

            LogicMouse = null;
            RejectedLogicKeys = new HashSet<Keys>();
            AddedLogicKeys = new HashSet<Keys>();
        }

        public static bool Update()
        {
            // get the actual input data loaded
            RealInputState.Update();
            UpdateOverlays();

            // check if prior state or current keyboard should advance
            bool storedInputAdvance = HandleStoredInput();
            bool realInputAdvance = HandleRealInput();
            bool gameLogicAdvance = HandleGameLogicInput();
            // TODO: only call when a menu is up?
            HandleTextBoxEntry();
            
            if (!storedInputAdvance)
            {
                // write new frame inputs if they exist
                if (gameLogicAdvance)
                {
                    SInputState.SetMouse(RealInputState.mouseState, LogicMouse);
                    SInputState.SetKeyboard(RealInputState.keyboardState, AddedLogicKeys, RejectedLogicKeys);

                    State.FrameStates.Add(new FrameState(SInputState.GetKeyboard(), SInputState.GetMouse()));
                }
                else if (realInputAdvance)
                {
                    SInputState.SetMouse(RealInputState.mouseState, RealMouse);
                    SInputState.SetKeyboard(RealInputState.keyboardState, AddedRealKeys, RejectedRealKeys);

                    State.FrameStates.Add(new FrameState(SInputState.GetKeyboard(), SInputState.GetMouse()));
                }
            }
            else
            {
                // pull frame data from state list
                State.FrameStates[(int)DateTime.CurrentFrame].toStates(out SInputState.kState, out SInputState.mState);
            }
            // set flag to ensure input is pulled from state
            SInputState.Active = realInputAdvance || gameLogicAdvance || storedInputAdvance;
            return SInputState.Active;
        }

        public static void UpdateOverlays()
        {
            foreach (var overlay in Overlays.Values)
            {
                overlay.Update();
            }
        }
        public static void Draw()
        {
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            foreach (var overlay in Overlays.Values)
            {
                overlay.Draw();
            }
            Game1.spriteBatch.End();
        }

        private static bool HandleRealInput()
        {
            RejectedRealKeys.Clear();
            AddedRealKeys.Clear();
            RealMouse = null;

            // frame advance
            bool advance = false;
            if (!SGame.console.IsOpen)
            {
                if (RealInputState.KeyTriggered(Keys.Q))
                {
                    advance = true;
                    RejectedRealKeys.Add(Keys.Q);
                }
                if (RealInputState.KeyTriggered(Keys.Down))
                {
                    advance = true;
                    RejectedRealKeys.Add(Keys.Down);
                }
                if (RealInputState.IsKeyDown(Keys.Space))
                {
                    advance = true;
                    RejectedRealKeys.Add(Keys.Space);
                }
                // Anim Cancel
                if (RealInputState.KeyTriggered(Keys.R))
                {
                    advance = true;
                    AddedRealKeys.Add(Keys.R);
                    AddedRealKeys.Add(Keys.RightShift);
                    AddedRealKeys.Add(Keys.Delete);
                }
                // quit
                if ((RealInputState.KeyTriggered(Keys.OemOpenBrackets) && RealInputState.IsKeyDown(Keys.OemCloseBrackets)) ||
                    (RealInputState.IsKeyDown(Keys.OemOpenBrackets) && RealInputState.KeyTriggered(Keys.OemCloseBrackets)))
                {
                    Program.gamePtr.Exit();
                }

                // reset
                if (RealInputState.KeyTriggered(Keys.Home))
                {
                    Program.gamePtr.KillGame1(true);
                    advance = false;
                }
                if (RealInputState.KeyTriggered(Keys.End))
                {
                    Program.gamePtr.KillGame1(false);
                    advance = false;
                }

                // save/load
                if (RealInputState.KeyTriggered(Keys.OemPeriod))
                    State.Save();
                if (RealInputState.KeyTriggered(Keys.OemComma))
                    State = SaveState.Load(State.Prefix);
            }
            return advance;
        }

        private static bool HandleGameLogicInput()
        {
            AddedLogicKeys.Clear();
            RejectedLogicKeys.Clear();
            LogicMouse = null;
            foreach(IGameLogic logic in GameLogics.Values)
            {
                if (logic.Update(out SKeyboardState keys, out SMouseState mouse))
                {
                    if (keys != null)
                        AddedLogicKeys.UnionWith(keys);
                    if (mouse != null)
                        LogicMouse = mouse;
                    return true;
                }
            }
            return false;
        }

        private static bool HandleStoredInput()
        {
            if (State.FrameStates.IndexInRange((int)DateTime.CurrentFrame))
            {
                return true;
            }
            return false;
        }

        private static void HandleTextBoxEntry()
        {
            TextBox textBox = TextBoxInput.GetSelected(out string Name);
            if (textBox != null)
            {
                if (textBox.Text.Length == 0)
                {
                    switch(Name)
                    {
                        case "nameBox":
                            TextBoxInput.Write(textBox, State.FarmerName);
                            break;
                        case "farmnameBox":
                            TextBoxInput.Write(textBox, State.FarmName);
                            break;
                        case "favThingBox":
                            TextBoxInput.Write(textBox, State.FavoriteThing);
                            break;
                        default:
                            TextBoxInput.Write(textBox, "abc");
                            break;
                    }
                }
            }
        }

        public static SMouseState LastFrameMouse()
        {
            if (DateTime.CurrentFrame == 0)
            {
                return new SMouseState();
            }
            State.FrameStates[(int)DateTime.CurrentFrame - 1].toStates(out _, out SMouseState mouse);
            return mouse;
        }

        public static void Reset()
        {
            DateTime.setUniqueIDForThisGame((ulong)State.Seed);
            UpdateWrapper.Reset();
            DrawWrapper.Reset();
            SInputState.Reset();
            DateTime.Reset();
        }
    }
}
