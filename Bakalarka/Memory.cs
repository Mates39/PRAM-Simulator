using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class Memory
    {
        public List<int> memory;
        public List<MemoryAccess> MemoryAccesses { get; set; }
        public Memory(List<int> mem) { this.memory = mem; }
        public Memory() { 
            this.memory = new List<int>();
            this.MemoryAccesses = new List<MemoryAccess>();
        }
        public void TryWrite(MemoryAccess ma)
        {
            MemoryAccesses.Add(ma);
            //memory[ma.memoryIndex] = ma.value;
        }
    }
}
