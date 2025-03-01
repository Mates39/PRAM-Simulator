using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class LocalMemoryGateway : IGateway
    {
        public Memory memory { get; set; }
        public int index { get; set; }
        public LocalMemoryGateway(Memory mem, int ind) { this.memory = mem; this.index = ind; }
        public LocalMemoryGateway() { }
    }
}
