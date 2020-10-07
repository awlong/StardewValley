using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TAS
{
    public class StateList : List<FrameState>
    {
        public void Pop() { RemoveAt(Count - 1); }
        
        // Dump all frames above the reset threshold
        public void Reset(int lastFrame = -1)
        {
            if (lastFrame == -1 || lastFrame > Count)
                lastFrame = Count;
            while (Count > lastFrame)
                Pop();
        }

        public bool IndexInRange(int index)
        {
            return 0 <= index && index < Count;
        }
    }
}
