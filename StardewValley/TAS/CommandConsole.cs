using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TAS.Commands;
using TAS.Inputs;
using TAS.Wrappers;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace TAS
{
    public class CommandConsole
    {
        public struct TextElement
        {
            public bool entry;
            public string text;
            public TextElement(string _text, bool _entry)
            {
                text = _text;
                entry = _entry;
            }
        }

        private const int leftPad = 5;

        private Rectangle historyRect;
        private Rectangle entryRect;
        public float openHeight = 0f;
        public float openHeightTarget = 0f;
        public float openHeightMax = 0.5f;
        public float openRate = 0.04f;

        private float fontSize = 1f;

        private Color backgroundHistoryColor = new Color(10, 10, 10, 220);
        private Color textHistoryColor = new Color(180, 180, 180, 255);
        private Color backgroundEntryColor = new Color(40, 40, 40, 220);
        private Color textEntryColor = new Color(100, 180, 180, 255);
        private Color cursorColor = new Color(180, 180, 180, 128);

        public SpriteFont consoleFont;
        public Texture2D solidColor;
        private SpriteBatch spriteBatch;

        private List<TextElement> historyLog;
        private int historyIndex;
        private int historyRectRows;
        private int historyTail;

        private string entryText = "";
        private int cursorPosition;

        public Dictionary<string, ICommand> Commands;
        public Dictionary<string, string> Aliases;
        public Stack<string> ActiveSubscribers = new Stack<string>();

        public CommandConsole()
        {
            historyLog = new List<TextElement>();
            consoleFont = SGame.content.Load<SpriteFont>("Fonts\\ConsoleFont");
            solidColor = new Texture2D(SGame.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] data = new Color[1] { new Color(255, 255, 255, 255) };
            solidColor.SetData(data);

            spriteBatch = new SpriteBatch(SGame.graphics.GraphicsDevice);
            KeyboardInput.CharEntered += EventInput_CharEntered;

            Commands = new Dictionary<string, ICommand>();
            foreach (var v in Reflector.GetTypesInNamespace(ResetWrapper.ExecutingAssembly, "TAS.Commands"))
            {
                if (v.IsAbstract || v.BaseType != typeof(ICommand))
                    continue;
                ICommand command = (ICommand)Activator.CreateInstance(v);
                Commands.Add(command.Name, command);
                Debug.WriteLine("Command \"{0}\" added to commands list", command.Name);
            }

            Aliases = new Dictionary<string, string>();
        }




        public bool IsOpen => openHeight > 0;

        public bool IsOpenMax => openHeight == openHeightMax;

        public void Open() { if (!IsOpenMax) openHeightTarget = openHeightMax; }
        public void Close() { if (IsOpen) openHeightTarget = 0; }

        public void Update()
        {
            if (RealInputState.KeyTriggered(Keys.OemTilde))
            {
                if (IsOpen)
                    Close();
                else
                    Open();
            }
            else if (IsOpenMax)
            {
                // do keyboard stuff with the history
                if (RealInputState.KeyTriggered(Keys.Up))
                {
                    BackHistory();
                }
                else if (RealInputState.KeyTriggered(Keys.Down))
                {
                    ForwardHistory();
                }
                else if (RealInputState.KeyTriggered(Keys.Left))
                {
                    if (RealInputState.IsKeyDown(Keys.LeftControl))
                    {
                        string[] tokens = entryText.Split(' ');
                        int count = 0;
                        foreach (string token in tokens)
                        {
                            if (count + token.Length + 1 >= cursorPosition)
                            {
                                break;
                            }
                            count += token.Length + 1;
                        }
                        // clamp
                        cursorPosition = Math.Max(0, count);
                    }
                    else
                    {
                        cursorPosition = Math.Max(0, cursorPosition - 1);
                    }
                }
                else if (RealInputState.KeyTriggered(Keys.Right))
                {
                    if (RealInputState.IsKeyDown(Keys.LeftControl))
                    {
                        string[] tokens = entryText.Split(' ');
                        int count = 0;
                        foreach (string token in tokens)
                        {
                            if (count > cursorPosition)
                            {
                                break;
                            }
                            count += token.Length + 1;
                        }
                        // clamp
                        cursorPosition = Math.Min(count, entryText.Length);
                    }
                    else
                    {
                        cursorPosition = Math.Min(entryText.Length, cursorPosition + 1);
                    }
                }
            }

            if (RealInputState.ScrollWheelTriggered())
            {
                // scroll
                if (historyLog.Count > historyRectRows)
                {
                    historyTail -= RealInputState.ScrollWheelDiff();
                    historyTail = Math.Min(Math.Max(historyRectRows - 1, historyTail), historyLog.Count);
                }
                else
                {
                    historyTail = historyLog.Count;
                }
            }

            openHeight += Math.Sign(openHeightTarget - openHeight) * openRate;
            openHeight = Math.Min(openHeightMax, Math.Max(openHeight, 0));

            historyRect.Width = SGame.graphics.GraphicsDevice.Viewport.Width;
            historyRect.Height = (int)(openHeight * SGame.graphics.GraphicsDevice.Viewport.Height);
            historyRectRows = historyRect.Height / consoleFont.LineSpacing;
            entryRect.Width = SGame.graphics.GraphicsDevice.Viewport.Width;
            entryRect.Height = consoleFont.LineSpacing * 3 / 2;
            entryRect.Y = historyRect.Height;
        }

        public void Draw()
        {
            if (IsOpen)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
                // draw the history block
                Vector2 historyLoc = new Vector2(leftPad, historyRect.Height - 1.25f * consoleFont.LineSpacing);
                spriteBatch.Draw(solidColor, historyRect, null, backgroundHistoryColor, 0, Vector2.Zero, SpriteEffects.None, 0);
                int index = historyTail - 1;
                while (historyLoc.Y + consoleFont.LineSpacing > 0 && index >= 0)
                {
                    string text = historyLog[index].text;
                    text = text.Replace("\t", "    ");
                    spriteBatch.DrawString(consoleFont,
                        text,
                        historyLoc,
                        historyLog[index].entry ? textEntryColor : textHistoryColor,
                        0f, Vector2.Zero, fontSize, SpriteEffects.None, 0.999999f
                        );
                    historyLoc.Y -= consoleFont.LineSpacing;
                    index--;
                }

                // draw the entry block
                spriteBatch.Draw(solidColor, entryRect, null, backgroundEntryColor, 0, Vector2.Zero, SpriteEffects.None, 0);
                Vector2 entryLoc = new Vector2(leftPad, entryRect.Y);
                spriteBatch.DrawString(consoleFont, entryText, entryLoc, textEntryColor, 0f, Vector2.Zero, fontSize, SpriteEffects.None, 0.999999f);

                // draw the cursor position
                Vector2 characterSize = consoleFont.MeasureString(" ") * fontSize;
                Rectangle cursorRect = new Rectangle((int)(entryLoc.X + characterSize.X * cursorPosition), (int)entryLoc.Y, (int)characterSize.X, (int)characterSize.Y);
                spriteBatch.Draw(solidColor, cursorRect, null, cursorColor, 0, Vector2.Zero, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }


        private void ResetEntry()
        {
            entryText = "";
            cursorPosition = 0;
        }
        private void ResetHistoryPointers()
        {
            historyIndex = historyLog.Count;
            historyTail = historyLog.Count;
        }
        private void BackHistory()
        {
            if (cursorPosition != entryText.Length)
            {
                cursorPosition = entryText.Length;
                return;
            }
            while (--historyIndex >= 0 && historyLog.Count != 0)
            {
                if (historyLog[historyIndex].entry)
                {
                    entryText = historyLog[historyIndex].text;
                    cursorPosition = entryText.Length;
                    return;
                }
            }
            ResetEntry();
            historyIndex = historyLog.Count;
        }
        private void ForwardHistory()
        {
            while (++historyIndex < historyLog.Count)
            {
                if (historyLog[historyIndex].entry)
                {
                    entryText = historyLog[historyIndex].text;
                    cursorPosition = entryText.Length;
                    return;
                }
            }
            ResetEntry();
            historyIndex = historyLog.Count;
        }

        private void EventInput_CharEntered(object sender, CharacterEventArgs e)
        {
            if (IsOpenMax)
            {
                if (char.IsControl(e.Character))
                {
                    ReceiveCommandInput(e.Character);
                }
                else
                {
                    ReceiveTextInput(e.Character);
                }
            }
        }

        private void ReceiveCommandInput(char command)
        {
            switch (command)
            {
                case '\b':
                    // backspace
                    if (entryText.Length > 0)
                    {
                        if (cursorPosition > 0)
                        {
                            entryText = entryText.Remove(cursorPosition - 1, 1);
                        }
                        cursorPosition = Math.Max(0, cursorPosition - 1);
                    }
                    break;
                case '\r':
                    // return
                    PushCommand(entryText);
                    ResetEntry();
                    ResetHistoryPointers();
                    break;
                case '\t':
                    Debug.WriteLine("Pressed Command Key TAB: {0}", command);
                    break;
                default:
                    Debug.WriteLine("Pressed Command Key: {0}", command);
                    break;
            }
        }


        private void ReceiveTextInput(char character)
        {
            switch (character)
            {
                // don't handle the tilde case
                case '~':
                case '`':
                    break;
                default:
                    entryText = entryText.Insert(cursorPosition++, character.ToString());
                    ResetHistoryPointers();
                    break;
            }
        }

        public bool HandleSubscribers(string command)
        {
            if (ActiveSubscribers.Count > 0)
            {
                string name = ActiveSubscribers.Peek();
                Commands[name].ReceiveInput(command);
                return true;
            }
            return false;
        }
        public void PushCommand(string command)
        {
            if (HandleSubscribers(command))
                return;

            if (command != "")
            {
                historyLog.Add(new TextElement(command, true));
                RunCommand(command);
            }
        }
        public void PushResult(string result)
        {
            historyLog.Add(new TextElement(result, false));
        }
        public void RunCommand(string command)
        {
            if (Aliases.ContainsKey(command))
            {
                RunCommand(Aliases[command]);
                return;
            }
            string[] tokens = command.Trim().Split(' ');
            string func = tokens[0];
            string[] parameters = tokens.Skip(1).ToArray();

            if (Commands.ContainsKey(func))
            {
                Commands[func].Run(parameters);
            }
        }


    }
}
