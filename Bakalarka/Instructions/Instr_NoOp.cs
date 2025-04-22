using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal class Instr_NoOp : IInstruction
    {
        public int InstructionPointer { get; set; }
        public string InstructionString { get; set; }
        public int CodeLineIndex { get; set; }
        public Instr_NoOp(int IP) { InstructionPointer = IP; }

        public int Execute(int procID)
        {
            return InstructionPointer + 1;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_NoOp(InstructionPointer);
        }
    }
}
