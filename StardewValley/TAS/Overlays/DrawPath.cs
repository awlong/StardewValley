using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Overlays
{
    public class DrawPath : IOverlay
    {
        public override string Name => "drawpath";
        public Color color = Color.LightCyan;
        public int thickness = 8;
        public override string[] HelpText()
        {
            return new string[] { string.Format("{0}: draw active path", Name) };
        }

        public override void ActiveDraw(SpriteBatch spriteBatch)
        {
            if (SGame.pathFinder.hasPath && SGame.pathFinder.path != null)
            {
                for(int i = 0; i < SGame.pathFinder.path.Count - 1; i++)
                {
                    DrawLineBetweenTiles(spriteBatch, SGame.pathFinder.path[i].toVector2(), SGame.pathFinder.path[i+1].toVector2(), color, thickness);
                }
            }
        }
    }
}
