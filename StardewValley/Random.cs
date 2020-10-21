using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using TAS;
namespace StardewValley
{
    public class Random
    {
        private System.Random _random;

        public Random()
        {
            int seed = (int)(DateTime.UtcNow - DateTime.Epoch).TotalSeconds;
            _random = new System.Random(seed);
        }

        public Random(int seed)
        {
            _random = new System.Random(seed);
        }
        public Random(Random other)
        {
            _random = new System.Random();
            SeedArray = other.SeedArray;
            inext = other.inext;
            inextp = other.inextp;
        }

        public Random Copy()
        {
            return new Random(this);
        }

        public int PeekNext() { return Copy().Next(); }
        public int Next() { return _random.Next(); }
        public int Next(int maxValue) { return _random.Next(maxValue); }
        public int Next(int minValue, int maxValue) { return _random.Next(minValue, maxValue); }
        public void NextBytes(byte[] buffer) { _random.NextBytes(buffer); }
        public Double NextDouble() { return _random.NextDouble(); }


        protected int[] SeedArray
        {
            get
            {
                return (int[])Reflector.GetValue(_random, "SeedArray");
            }
            set
            {
                Reflector.SetValue(_random, "SeedArray", value.Clone());
            }
        }

        protected int inext
        {
            get
            {
                return (int)Reflector.GetValue(_random, "inext");
            }
            set
            {
                Reflector.SetValue(_random, "inext", value);
            }
        }

        protected int inextp
        {
            get
            {
                return (int)Reflector.GetValue(_random, "inextp");
            }
            set
            {
                Reflector.SetValue(_random, "inextp", value);
            }
        }

        public static bool operator==(Random lhs, Random rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                return Object.ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }
        
        public static bool operator!=(Random lhs, Random rhs)
        {
            return !(lhs == rhs);
        }
       
        public bool Equals(Random o)
        {
            if (Object.ReferenceEquals(o, null))
                return false;
            if (Object.ReferenceEquals(this, o))
                return true;
            if (this.inext == o.inext && this.inextp == o.inextp)
            {
                return SeedArray.SequenceEqual(o.SeedArray);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Random))
                return false;
            return this == (Random)obj;
        }

        public override int GetHashCode()
        {
            return _random.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("inext: '{0}' ", inext);
            sb.AppendFormat("inextp: '{0}' ", inextp);
            return sb.ToString();
        }
    }
}
