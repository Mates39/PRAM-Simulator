using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal class Instr_Halt : IInstruction
    {
        public int InstructionPointer { get; set; }
        public Instr_Halt(int IP) { InstructionPointer = IP; }
        public int Execute(int procID)
        {
            return -1;
        }
    }
}
