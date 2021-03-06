﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using TAS.Wrappers;

using SpriteBatch = StardewValley.SpriteBatch;
using DateTime = StardewValley.DateTime;
using TAS.Utilities;
using Newtonsoft.Json;
using Steamworks;

namespace TAS
{
    public class SGame : Game
    {
        public Game1 game;
        public static GraphicsDeviceManager graphics;
        public static IAudioEngine audioEngine;
        public static WaveBank waveBank;
        public static WaveBank waveBank1_4;
        public static ISoundBank soundBank;
        public static KeyboardDispatcher keyboardDispatcher;
        public static bool ResetGame;
        public static bool FastAdvance;
        public static int ReplayNormalFrames = 30;
        public static int ApproxFastFramesBetweenRender = 30;
        public static CommandConsole console;
        public static LocalizedContentManager content;
        public static PathFinder pathFinder;

        public SGame()
        {
            // moved from Game1()
            Program.gamePtr = this;
            Program.sdk.EarlyInitialize();
            if (!Program.releaseBuild)
            {
                base.InactiveSleepTime = new TimeSpan(0L);
            }

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            base.Window.AllowUserResizing = false;
            base.Content.RootDirectory = "Content";
            base.Exiting += OnExiting;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            pathFinder = new PathFinder();
        }

        protected override void Initialize()
        {
            // moved from Game1.Initialize()
            keyboardDispatcher = new KeyboardDispatcher(base.Window);
            base.IsFixedTimeStep = true;
            string rootpath = base.Content.RootDirectory;
            File.Exists(Path.Combine(rootpath, "XACT", "FarmerSounds.xgs"));
            try
            {
                audioEngine = new AudioEngineWrapper(new AudioEngine(Path.Combine(rootpath, "XACT", "FarmerSounds.xgs")));
                waveBank = new WaveBank(audioEngine.Engine, Path.Combine(rootpath, "XACT", "Wave Bank.xwb"));
                waveBank1_4 = new WaveBank(audioEngine.Engine, Path.Combine(rootpath, "XACT", "Wave Bank(1.4).xwb"));
                soundBank = new SoundBankWrapper(new SoundBank(audioEngine.Engine, Path.Combine(rootpath, "XACT", "Sound Bank.xsb")));
            }
            catch (Exception)
            {
                audioEngine = new DummyAudioEngine();
                soundBank = new DummySoundBank();
            }
            audioEngine.Update();
            base.Initialize();
            graphics.SynchronizeWithVerticalRetrace = true;
            Program.sdk.Initialize();

            content = new LocalizedContentManager(base.Content.ServiceProvider, base.Content.RootDirectory);
            console = new CommandConsole();

            LoadEngineState();
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            // ensure Game1 gets an update tick
            base.OnActivated(sender, args);
            game?.OnActivated(sender, args);
        }

        protected override void Update(GameTime gameTime)
        {
            console.Update();
            if(game == null || ResetGame)
            {
                ResetGame = false;
                SetupGame1();
            }
            if (FastAdvance)
            {
                FastAdvance = false;
                RunFast();
            }

            UpdateGame(ref gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            DrawGame(ref gameTime);
            console.Draw();
            base.Draw(gameTime);
        }

        private void UpdateGame(ref GameTime gameTime)
        {
            if (UpdateWrapper.Prefix(ref gameTime))
            {
                game.Update(gameTime);
            }
            UpdateWrapper.Postfix(gameTime);
        }
        private void DrawGame(ref GameTime gameTime)
        {
            if (DrawWrapper.Prefix(ref gameTime))
            {
                game?.Draw(gameTime);
            }
            DrawWrapper.Postfix(gameTime);
        }

        private void RunFast()
        {
            int counter = 0;
            SpriteBatch.Active = false;
            while ((int)DateTime.CurrentFrame + ReplayNormalFrames < Controller.State.Count)
            {
                GameTime gameTime = DateTime.CurrentGameTime;
                UpdateGame(ref gameTime);
                DrawGame(ref gameTime);
                if (counter++ >= ApproxFastFramesBetweenRender)
                    break;
            }
            SpriteBatch.Active = true;
            // enable the draw for 1 frame
            if (counter >= ApproxFastFramesBetweenRender)
                FastAdvance = true;
        }

        public void SetupGame1()
        {
            // clear the old game state completely
            game?.exitEvent();
            game?.UnloadContent();
            ResetWrapper.Reset();

            // preload static vars for game1
            // lambda redirects were super slow in debug
            Game1.graphics = graphics;
            Game1.GraphicsDevice = GraphicsDevice;
            Game1.Content = base.Content;
            Game1.audioEngine = audioEngine;
            Game1.waveBank = waveBank;
            Game1.waveBank1_4 = waveBank1_4;
            Game1.soundBank = soundBank;
            Game1.keyboardDispatcher = keyboardDispatcher;
            Game1.Window = base.Window;

            game = new Game1();
            LocalizedContentManager.CurrentLanguageCode = Controller.State.Language;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            // might as well fail out
            Process.GetCurrentProcess().Kill();
        }

        public void KillGame1(bool fastAdvance = false)
        {
            ResetGame = true;
            FastAdvance = fastAdvance;
        }

        public void SaveEngineState()
        {
            EngineState state = new EngineState();
            string filePath = Path.Combine(Constants.BasePath, "engine_state.json");
            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, state);
            }
        }

        public void LoadEngineState()
        {
            string filePath = Path.Combine(Constants.BasePath, "engine_state.json");
            if (!File.Exists(filePath))
                return;
            
            EngineState state = null;
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                // TODO: any safety rails for overwriting current State?
                state = (EngineState)serializer.Deserialize(file, typeof(EngineState));
            }
            state.UpdateGame();
        }
        private class EngineState
        {
            public Dictionary<string, string> Aliases;
            public Dictionary<string, bool> OverlayState;
            public Dictionary<string, bool> GameLogicState;

            public EngineState()
            {
                Aliases = new Dictionary<string, string>(console.Aliases);
                OverlayState = new Dictionary<string, bool>();
                foreach (var overlay in Controller.Overlays)
                {
                    OverlayState.Add(overlay.Key, overlay.Value.Active);
                }
                GameLogicState = new Dictionary<string, bool>();
                foreach (var logic in Controller.GameLogics)
                {
                    GameLogicState.Add(logic.Key, logic.Value.Active);
                }
            }

            public void UpdateGame()
            {
                console.Aliases = new Dictionary<string, string>(Aliases);
                foreach (var overlay in OverlayState)
                {
                    if (Controller.Overlays.ContainsKey(overlay.Key))
                        Controller.Overlays[overlay.Key].Active = overlay.Value;
                }
                foreach (var logic in GameLogicState)
                {
                    if (Controller.GameLogics.ContainsKey(logic.Key))
                        Controller.GameLogics[logic.Key].Active = logic.Value;
                }
            }
        }
    }
}
