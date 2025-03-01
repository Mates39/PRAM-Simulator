using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    public class Instr_Read : IInstruction
    {
        public Instr_Read(int address) { this.address = address; }
        public int InstructionPointer { get; set; }
        public int address;
        public int value;
        public int Execute(int procID)
        {
            return 1;
        }
    }
}
