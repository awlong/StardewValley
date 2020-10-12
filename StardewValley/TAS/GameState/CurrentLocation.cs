using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.GameState
{
    public class CurrentLocation
    {
        public static bool Active { get { return Game1.currentLocation != null; } }
    }
}
