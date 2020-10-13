using Microsoft.Xna.Framework;
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
        public static CommandConsole console;
        public static LocalizedContentManager content;

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
            SpriteBatch.Active = false;
            while ((int)DateTime.CurrentFrame + ReplayNormalFrames < Controller.State.Count)
            {
                GameTime gameTime = DateTime.CurrentGameTime;
                UpdateGame(ref gameTime);
                DrawGame(ref gameTime);
            }
            SpriteBatch.Active = true;
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
    }
}
