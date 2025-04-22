using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal class Instr_ParallelEnd : IInstruction
    {
        public int InstructionPointer { get; set; }
        public string InstructionString { get; set; }
        public int CodeLineIndex { get; set; }
        public PRAM pram { get; set; }
        public Instr_ParallelEnd(int instructionPointer, PRAM pram, int codeLineIndex)
        {
            InstructionPointer = instructionPointer;
            this.pram = pram;
            CodeLineIndex = codeLineIndex;
        }
        public int Execute(int procCount)
        {
            return InstructionPointer + 1;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_ParallelEnd(InstructionPointer, pram, CodeLineIndex);
        }
    }
}
