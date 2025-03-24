using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class MemoryGateway
    {
        private IGateway gateway;
        private int index;
        public MemoryGateway(IGateway gat, int ind) { this.gateway = gat; this.index = ind; }

        public int Read()
        {
            return gateway.Read(index);
        }
        public int Read(int index)
        {
            return gateway.Read(index);
        }
        public void Write(int value, int procID)
        {
            gateway.Write(index, value, procID);
        }
        public void Write(int index, int value, int procID)
        {
            gateway.Write(index, value, procID);
        }
        public MemoryGateway Duplicate(LocalMemoryGateway localGateway)
        {
            if (gateway is SharedMemoryGateway)
                return new MemoryGateway(gateway, index);
            else
                return new MemoryGateway(localGateway, index);
        }
    }
}
