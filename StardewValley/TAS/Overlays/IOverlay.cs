using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        public void Update() { if (Active) { ActiveUpdate(); } }
        public virtual void ActiveUpdate() { }

        public void Draw() { if (Active) { ActiveDraw(Game1.spriteBatch); } }
        public virtual void ActiveDraw(SpriteBatch spriteBatch) { }

        public virtual void Reset() { }

        private static Rectangle? OutlineRect;

        protected Vector2 MeasureString(string text, float scale=1)
        {
            return Font.MeasureString(text) * scale;
        }
        protected void DrawText(SpriteBatch spriteBatch, string text, Vector2 vector, Color textColor, Color backgroundColor, float fontScale = 1)
        {
            // measure font and offset vector if drawing offscreen
            Vector2 textSize = MeasureString(text, fontScale);
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

        protected void DrawText(SpriteBatch spriteBatch, IEnumerable<string> text, Vector2 vector, Color textColor, Color backgroundColor, float fontScale = 1)
        {
            float maxWidth = 0;
            float height = 0;
            float startX = vector.X;
            float startY = vector.Y;
            foreach (var line in text)
            {
                Vector2 textSize = MeasureString(line, fontScale);
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

        protected void DrawTextAtTile(SpriteBatch spriteBatch, string text, Vector2 tile, Color textColor, Color backgroundColor, float fontScale = 1)
        {
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, tile * Game1.tileSize);
            DrawText(spriteBatch, text, local, textColor, backgroundColor, fontScale);
        }

        protected void DrawTextAtTile(SpriteBatch spriteBatch, IEnumerable<string> text, Vector2 tile, Color textColor, Color backgroundColor, float fontScale = 1)
        {
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, tile * Game1.tileSize);
            DrawText(spriteBatch, text, local, textColor, backgroundColor, fontScale);
        }

        // draw at on screen coord
        protected void DrawObjectSprite(SpriteBatch spriteBatch, Vector2 coord, int parentSheetIndex)
        {
            DrawObjectSprite(spriteBatch, coord, new Vector2(Game1.tileSize, Game1.tileSize), parentSheetIndex);
        }

        // draw over on screen tile
        protected void DrawObjectSpriteAtTile(SpriteBatch spriteBatch, Vector2 tile, int parentSheetIndex)
        {
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, tile * Game1.tileSize);
            DrawObjectSprite(spriteBatch, local, new Vector2(Game1.tileSize, Game1.tileSize), parentSheetIndex);
        }
        protected void DrawObjectSprite(SpriteBatch spriteBatch, Vector2 start, Vector2 dim, int parentSheetIndex)
        {
            Rectangle sourceRect = GameLocation.getSourceRectForObject(parentSheetIndex);
            Rectangle destRect = new Rectangle((int)start.X, (int)start.Y, (int)dim.X, (int)dim.Y);
            DrawObjectSprite(spriteBatch, sourceRect, destRect);
        }

        protected void DrawObjectSprite(SpriteBatch spriteBatch, Rectangle sourceRect, Rectangle destRect)
        {
            spriteBatch.Draw(Game1.objectSpriteSheet, destRect, sourceRect, Color.White);
        }

        protected void DrawLineGlobal(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness = 1)
        {
            Vector2 startCoord = Game1.GlobalToLocal(start);
            Vector2 endCoord = Game1.GlobalToLocal(end);
            DrawLineLocal(spriteBatch, startCoord, endCoord, color, thickness);
        }
        protected void DrawLineBetweenTiles(SpriteBatch spriteBatch, Vector2 startTile, Vector2 endTile, Color color, int thickness = 1)
        {
            Vector2 startCoord = Game1.GlobalToLocal(Game1.viewport, (startTile + new Vector2(0.5f, 0.5f)) * Game1.tileSize);
            Vector2 endCoord = Game1.GlobalToLocal(Game1.viewport, (endTile + new Vector2(0.5f, 0.5f)) * Game1.tileSize);
            DrawLineLocal(spriteBatch, startCoord, endCoord, color, thickness);
        }
        protected void DrawLineLocalToGlobal(SpriteBatch spriteBatch, Vector2 local, Vector2 global, Color color, int thickness = 1)
        {
            Vector2 globalCoord = Game1.GlobalToLocal(global);
            DrawLineLocal(spriteBatch, local, globalCoord, color, thickness);
        }
        protected void DrawLineLocalToTile(SpriteBatch spriteBatch, Vector2 local, Vector2 tile, Color color, int thickness = 1)
        {
            Vector2 tileCoord = Game1.GlobalToLocal(Game1.viewport, (tile + new Vector2(0.5f, 0.5f)) * Game1.tileSize);
            DrawLineLocal(spriteBatch, local, tileCoord, color, thickness);
        }
        protected void DrawLineLocal(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int thickness = 1)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(SolidColor,
                new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), thickness),
                null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        protected void DrawLineTileToPlayer(SpriteBatch spriteBatch, Vector2 tile, Color color, int thickness = 1)
        {
            Vector2 tileCoord = Game1.GlobalToLocal(Game1.viewport, (tile + new Vector2(0.5f, 0.5f)) * Game1.tileSize);
            Vector2 playerCoord = Game1.GlobalToLocal(Game1.viewport, Utility.PointToVector2(Game1.player.GetBoundingBox().Center));
            DrawLineLocal(spriteBatch, playerCoord, tileCoord, color, thickness);
        }

        protected void DrawRectGlobal(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            Rectangle localRect = Game1.GlobalToLocal(Game1.viewport, rect);
            DrawRectLocal(spriteBatch, localRect, color);
        }
        protected void DrawRectGlobal(SpriteBatch spriteBatch, Rectangle rect, Color color, Color crossColor)
        {
            Rectangle localRect = Game1.GlobalToLocal(Game1.viewport, rect);
            DrawRectLocal(spriteBatch, localRect, color, crossColor);
        }
        protected void DrawRectLocal(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(SolidColor, rect, null, color);
        }
        protected void DrawRectLocal(SpriteBatch spriteBatch, Rectangle rect, Color color, Color crossColor)
        {
            spriteBatch.Draw(SolidColor, rect, null, color);
            DrawLineLocal(spriteBatch, new Vector2(rect.Left, rect.Center.Y), new Vector2(rect.Right, rect.Center.Y), crossColor);
            DrawLineLocal(spriteBatch, new Vector2(rect.Center.X, rect.Top), new Vector2(rect.Center.X, rect.Bottom), crossColor);
        }
        protected void DrawFilledTile(SpriteBatch spriteBatch, Vector2 tile, Color color)
        {
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, tile * Game1.tileSize);
            DrawRectLocal(spriteBatch, new Rectangle((int)local.X, (int)local.Y, Game1.tileSize, Game1.tileSize), color);
        }
        protected void DrawCenteredTextInRectGlobal(SpriteBatch spriteBatch, Rectangle rect, string text, Color color, float fontScale = 1, int shadowOffset = 0)
        {
            // measure font and offset vector if drawing offscreen
            Rectangle local = Game1.GlobalToLocal(Game1.viewport, rect);
            Vector2 textSize = MeasureString(text, fontScale);
            Vector2 pos = (new Vector2(local.Width - textSize.X, local.Height - textSize.Y) / 2) + new Vector2(local.X, local.Y);
            if (shadowOffset != 0)
                spriteBatch.DrawString(Font, text, new Vector2(pos.X + shadowOffset, pos.Y + shadowOffset), Color.Black, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Font, text, pos, color, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
        }
        protected void DrawCenteredTextInRectLocal(SpriteBatch spriteBatch, Rectangle rect, string text, Color color, float fontScale = 1, int shadowOffset = 0)
        {
            // measure font and offset vector if drawing offscreen
            Vector2 textSize = MeasureString(text, fontScale);
            Vector2 pos = (new Vector2(rect.Width - textSize.X, rect.Height - textSize.Y) / 2) + new Vector2(rect.X, rect.Y);
            if (shadowOffset != 0)
                spriteBatch.DrawString(Font, text, new Vector2(pos.X + shadowOffset, pos.Y + shadowOffset), Color.Black, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Font, text, pos, color, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
        }
        protected void DrawCenteredTextInTile(SpriteBatch spriteBatch, Vector2 tile, string text, Color color, float fontScale = 1, int shadowOffset = 1)
        {
            // measure font and offset vector if drawing offscreen
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, tile * Game1.tileSize);
            Rectangle rect = new Rectangle((int)local.X, (int)local.Y, Game1.tileSize, Game1.tileSize);
            Vector2 textSize = MeasureString(text, fontScale);
            Vector2 pos = (new Vector2(rect.Width - textSize.X, rect.Height - textSize.Y) / 2) + new Vector2(rect.X, rect.Y);
            spriteBatch.DrawString(Font, text, new Vector2(pos.X + shadowOffset, pos.Y + shadowOffset), Color.Black, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(Font, text, pos, color, 0f, Vector2.Zero, fontScale, SpriteEffects.None, 1f);
        }

        protected void DrawTileOutline(SpriteBatch spriteBatch, Vector2 tile, Color color, float scale = 1f)
        {
            if (OutlineRect == null)
                OutlineRect = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1));
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, tile * Game1.tileSize);
            spriteBatch.Draw(Game1.mouseCursors, local, OutlineRect, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
        }
        protected void DrawTileOutline(SpriteBatch spriteBatch, Vector2 tile, Color color, Vector2 scale)
        {
            if (OutlineRect == null)
                OutlineRect = new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 29, -1, -1));
            Vector2 local = Game1.GlobalToLocal(Game1.viewport, tile * Game1.tileSize);
            spriteBatch.Draw(Game1.mouseCursors, local, OutlineRect, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
        }

        protected void DrawRectFromTexture(SpriteBatch spriteBatch, Texture2D texture, Rectangle sourceRect, Rectangle destRect)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }
    }
}
