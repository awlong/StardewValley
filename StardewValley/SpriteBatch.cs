using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS;

namespace StardewValley
{
    public class SpriteBatch
    {
        public Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;
        public static bool Active = true;
        public bool inBeginEndPair => (bool)Reflector.GetValue(spriteBatch, "inBeginEndPair");
        public SpriteBatch(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(graphicsDevice);
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return spriteBatch.GraphicsDevice; }
        }

        public void Begin()
        {
            if (Active)
                spriteBatch.Begin();
        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            if (Active)
                spriteBatch.Begin(sortMode, blendState);
        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            if (Active)
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState);
        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            if (Active)
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect);
        }
        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            if (Active)
                spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }
        
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            if (Active)
                spriteBatch.Draw(texture, destinationRectangle, color);
        }
        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            if (Active)
                spriteBatch.Draw(texture, position, color);
        }
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            if (Active)
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            if (Active)
                spriteBatch.Draw(texture, position, sourceRectangle, color);
        }
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            if (Active)
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (Active)
                spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (Active)
                spriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            if (Active)
                spriteBatch.DrawString(spriteFont, text, position, color);
        }
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            if (Active)
                spriteBatch.DrawString(spriteFont, text, position, color);
        }
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (Active)
                spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            if (Active)
                spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (Active)
                spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (Active)
                spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void End()
        {
            if (Active)
                spriteBatch.End();
        }
        public void Dispose()
        {
            //spriteBatch.Dispose();
        }
    }
}
