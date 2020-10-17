using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.Collections;
using System.Collections.Generic;
using TAS.Components;
using TAS.GameState;
using SpriteBatch = StardewValley.SpriteBatch;

namespace TAS.Overlays
{
    public abstract class IOverlay : IConsoleAware
    {
        public abstract string Name { get; }
        public bool Active = false;
        public bool Toggle() { Active = !Active; return Active; }
        public Texture2D SolidColor { get { return SGame.console.solidColor; } }
        public SpriteFont Font { get { return SGame.console.consoleFont; } }
        public virtual void Update() { }
        public virtual void Reset() { }

        private static Rectangle? OutlineRect;

        public void Draw()
        {
            if (Active)
            {
                ActiveDraw(Game1.spriteBatch);
            }
        }

        public virtual void ActiveDraw(SpriteBatch spriteBatch) { }


        protected void DrawText(SpriteBatch spriteBatch, string text, Vector2 vector, Color textColor, Color backgroundColor, float fontScale=1)
        {
            // measure font and offset vector if drawing offscreen
            Vector2 textSize = Font.MeasureString(text) * fontScale;
            if (vector.X + textSize.X > Globals.ViewportWidth)
                vector.X = Globals.ViewportWidth - textSize.X;
            else if (vector.X < 0)
                vector.X = 0;

            if (vector.Y + textSize.Y > Globals.ViewportHeight)
                vector.Y = Globals.ViewportHeight - textSize.Y;
            else if (vector.Y < 0)
                vector.Y = 0;

            spriteBatch.Draw(SolidColor, new Rectangle((int)vector.X, (int)vector.Y, (int)textSize.X, (int)textSize.Y), backgroundColor);
            spriteBatch.DrawString(Font, text, vector, textColor, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
        }

        protected void DrawText(SpriteBatch spriteBatch, IEnumerable<string> text, Vector2 vector, Color textColor, Color backgroundColor, float fontScale=1)
        {
            float maxWidth = 0;
            float height = 0;
            float startX = vector.X;
            float startY = vector.Y;
            foreach (var line in text)
            {
                Vector2 textSize = Font.MeasureString(line) * fontScale;
                if (textSize.X > maxWidth)
                    maxWidth = textSize.X;
                height += textSize.Y;
            }
            if (startX + maxWidth > Globals.ViewportWidth)
                startX = Globals.ViewportWidth - maxWidth;
            else if (startX < 0)
                startX = 0;
            if (startY + height > Globals.ViewportHeight)
                startY = Globals.ViewportHeight - height;
            else if (startY < 0)
                startY = 0;

            spriteBatch.Draw(SolidColor, new Rectangle((int)startX, (int)startY, (int)maxWidth, (int)height), backgroundColor);
            Vector2 start = new Vector2(startX, startY);
            foreach (var line in text)
            {
                spriteBatch.DrawString(Font, line, start, textColor, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
                start.Y += Font.LineSpacing;
            }
        }
    }
}
