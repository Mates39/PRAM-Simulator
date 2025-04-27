using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal class Instr_Write : IInstruction
    {
        public Instr_Write(MemoryGateway gateway, int value) { this.value = value; this.gateway = gateway; }
        MemoryGateway gateway;
        public int value;

        public int InstructionPointer { get; set; }
        public string InstructionString { get; set; }
        public int CodeLineIndex { get; set; }
        public int Execute(int procID)
        {
            return InstructionPointer + 1;
        }

        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            throw new NotImplementedException();
        }
    }
}

