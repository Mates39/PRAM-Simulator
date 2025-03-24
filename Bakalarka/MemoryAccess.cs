using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    public class MemoryAccess
    {
        public IGateway gateway;
        public int processorID;
        public int memoryIndex;
        public int value;
        public MemoryAccess(IGateway gat, int procID, int memIndex, int val)
        {
            this.gateway = gat;
            this.processorID = procID;
            this.memoryIndex = memIndex;
            this.value = val;
        }
    }
}
