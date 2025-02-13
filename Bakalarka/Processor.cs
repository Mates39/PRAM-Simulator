using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class Processor
    {
        public int id;
        public int InstructionPointer = 0;
        public ProgramCode program;
        public List<int> localMemory = new List<int>();
        public Processor(int id, ProgramCode program)
        {
            this.id = id;
            this.program = program;
            localMemory = new List<int>();
        }
        public void ExecuteNextInstruction()
        {
            if (InstructionPointer >= 0)
            {
                InstructionPointer = program.instructions[InstructionPointer].Execute();
            }
        }

    }
}
