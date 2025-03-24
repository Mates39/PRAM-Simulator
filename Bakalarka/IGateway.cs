using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    public interface IGateway
    {
        Memory memory { get; set; }
        int Read(int index)
        {
            return memory.memory[index].Value;
        }
        void Write(int index, int value, int procID)
        {
            memory.TryWrite(new MemoryAccess(this, procID, index, value ));
        }
    }
}
