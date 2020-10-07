using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StardewValley
{
    public class DateTime
    {
        public static ulong CurrentFrame { get; set; }
        public static ulong FrameOffset { get; set; }
        public static TimeSpan FrameTimeSpan = new TimeSpan(166667);
        public static GameTime CurrentGameTime = new GameTime(new TimeSpan(), FrameTimeSpan);

        public static void setUniqueIDForThisGame(ulong id)
        {
            FrameOffset = id * 60;
        }

        public static ulong uniqueIdForThisGame
        {
            get
            {
                return (ulong)FrameOffset / 60;
            }
        }

        public static void Reset()
        {
            CurrentFrame = 0;
            CurrentGameTime = new GameTime(new TimeSpan(), FrameTimeSpan);
        }

        public static void Update()
        {
            CurrentFrame++;
            CurrentGameTime = new GameTime(CurrentGameTime.TotalGameTime.Add(FrameTimeSpan), FrameTimeSpan);
            Game1.currentGameTime = CurrentGameTime;
        }

        public static System.DateTime Epoch
        {
            get
            {
                return new System.DateTime(2012, 6, 22);
            }
        }

        public static System.DateTime EpochNow
        {
            get
            {
                System.DateTime ret = Epoch;
                ret = ret.AddMilliseconds(FrameTimeSpan.TotalMilliseconds * FrameOffset);
                return ret;
            }
        }

        public static System.DateTime Now
        {
            get
            {
                return new System.DateTime(Ticks);
            }
        }

        public static System.DateTime UtcNow
        {
            get
            {
                return new System.DateTime(Ticks);
            }
        }

        public static long Ticks
        {
            get
            {
                System.DateTime ret = EpochNow;
                ret = ret.AddMilliseconds(FrameTimeSpan.TotalMilliseconds * CurrentFrame);
                return ret.Ticks;
            }
        }

        public static int Ticks32
        {
            get
            {
                System.DateTime ret = EpochNow;
                ret = ret.AddMilliseconds(FrameTimeSpan.TotalMilliseconds * CurrentFrame);
                return (int)ret.Ticks;
            }
        }

        public static TimeSpan TimeOfDay
        {
            get
            {
                return Now.TimeOfDay;
            }
        }

        public static int Seconds
        {
            get
            {
                return (int)TimeOfDay.TotalSeconds;
            }
        }

        public static int Milliseconds
        {
            get
            {
                return (int)TimeOfDay.TotalMilliseconds;
            }
        }
    }
}
