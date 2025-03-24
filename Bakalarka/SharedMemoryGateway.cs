using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    public class SharedMemoryGateway : IGateway
    {
        public Memory memory { get; set; }
        public SharedMemoryGateway(Memory mem) { this.memory = mem; }
        public SharedMemoryGateway() { }
    }
}
