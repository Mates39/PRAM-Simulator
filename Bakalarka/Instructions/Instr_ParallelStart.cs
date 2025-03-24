using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal class Instr_ParallelStart : IInstruction
    {
        public int InstructionPointer { get; set; }
        public int procCount { get; set; }
        public MemoryGateway Gateway { get; set; }
        public PRAM pram { get; set; }
        public Instr_ParallelStart(int instructionPointer, PRAM pram, MemoryGateway gat)
        {
            InstructionPointer = instructionPointer;
            this.pram = pram;
            this.Gateway = gat;
        }
        public Instr_ParallelStart(int instructionPointer, PRAM pram, int procCount)
        {
            InstructionPointer = instructionPointer;
            this.pram = pram;
            this.procCount = procCount;
        }
        public int Execute(int id)
        {
            if(Gateway != null)
                pram.StartParallelExecution(Gateway.Read());
            else
                pram.StartParallelExecution(procCount);
            return InstructionPointer + 1;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            if (Gateway != null)
                return new Instr_ParallelStart(InstructionPointer, pram, Gateway);
            else
                return new Instr_ParallelStart(InstructionPointer, pram, procCount);
        }
    }
}
