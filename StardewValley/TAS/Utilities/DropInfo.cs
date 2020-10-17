using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAS.Utilities
{
    public class DropInfo
    {
        public static string ObjectName(int index)
        {
            if (!Game1.objectInformation.ContainsKey(index))
            {
                index = Math.Abs(index);
                switch(index)
                {
                    case 0:
                        index = 378;
                        break;
                    case 2:
                        index = 380;
                        break;
                    case 4:
                        index = 382;
                        break;
                    case 6:
                        index = 384;
                        break;
                    case 10:
                        index = 386;
                        break;
                    case 12:
                        index = 388;
                        break;
                    case 14:
                        index = 390;
                        break;
                    default:
                        return "unknown";
                }
            }
            return Game1.objectInformation[index].Split('/')[0];
        }
        public static string MultiDrop(int index, int num=1)
        {
            string drop = ObjectName(index);
            if (num == 1)
                return drop;
            return string.Format("{0} x{1}", drop, num);
        }
    }
}
