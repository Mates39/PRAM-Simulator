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
            if (memory.memory.Count <= index)
                throw new Exception("Cteni z indexu, ktery neexistuje");
            return memory.memory[index].Value;
        }
        void Write(int index, int value, int procID)
        {
            memory.TryWrite(new MemoryAccess(this, procID, index, value ));
        }
    }
}
