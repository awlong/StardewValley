using Microsoft.Xna.Framework;
using StardewValley;
using TAS.Components;

namespace TAS.Overlays
{
    public abstract class IOverlay : IConsoleAware
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

    }
}
