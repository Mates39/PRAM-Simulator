using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal interface IGateway
    {
        Memory memory { get; set; }
        int Read(int index)
        {
            return memory.memory[index];
        }
        void Write(int index, int value, int procID)
        {
            memory.TryWrite(new MemoryAccess() { processorID = procID, memoryIndex = index, value = value });
        }
    }
}
