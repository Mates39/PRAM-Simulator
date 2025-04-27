using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal class Instr_Read : IInstruction
    {
        public Instr_Read(MemoryGateway gateway, int address) { this.gateway = gateway;  this.address = address; }
        public int InstructionPointer { get; set; }
        public string InstructionString { get; set; }
        public int CodeLineIndex { get; set; }
        MemoryGateway gateway;
        public int address;
        public int Execute(int procID)
        {
            return gateway.Read(address);
        }

        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_Read(gateway.Duplicate(localGateway), address);
        }
    }
}
