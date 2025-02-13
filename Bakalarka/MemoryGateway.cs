using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class MemoryGateway
    {
        private List<int> memory;
        private int index;
        public MemoryGateway(List<int> mem, int ind) { this.memory = mem; this.index = ind; }

        public int Read()
        {
            return memory[index];
        }
        public int Read(int index)
        {
            return memory[index];
        }
        public void Write(int value)
        {
            memory[index] = value;
        }
        public void Write(int index, int value)
        {
            memory[index] = value;
        }
    }
}
