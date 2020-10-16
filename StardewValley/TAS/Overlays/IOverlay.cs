using Microsoft.Xna.Framework;
using StardewValley;

namespace TAS.Overlays
{
    public abstract class IOverlay
    {
        public abstract string Name { get; }
        public bool Active = false;
        public bool Toggle() { Active = !Active; return Active; }

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

        public virtual string[] HelpText()
        {
            return new string[] { string.Format(" \"{0}\": no help documentation", Name) };
        }
    }
}
