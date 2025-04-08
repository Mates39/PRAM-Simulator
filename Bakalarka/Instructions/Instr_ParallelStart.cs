using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakalarka.Expresions;

namespace Bakalarka.Instructions
{
    internal class Instr_ParallelStart : IInstruction
    {
        public int InstructionPointer { get; set; }
        public int CodeLineIndex { get; set; }
        public int procCount { get; set; }
        public IExpresion Expresion { get; set; }
        public MemoryGateway Gateway { get; set; }
        public PRAM pram { get; set; }
        public Instr_ParallelStart(int instructionPointer, PRAM pram, MemoryGateway gat, int codeLineIndex)
        {
            InstructionPointer = instructionPointer;
            this.pram = pram;
            this.Gateway = gat;
            CodeLineIndex = codeLineIndex;
        }
        public Instr_ParallelStart(int instructionPointer, PRAM pram, int procCount, int codeLineIndex)
        {
            InstructionPointer = instructionPointer;
            this.pram = pram;
            this.procCount = procCount;
            CodeLineIndex = codeLineIndex;
        }
        public Instr_ParallelStart(int instructionPointer, PRAM pram, IExpresion expr, int codeLineIndex)
        {
            InstructionPointer = instructionPointer;
            this.pram = pram;
            this.procCount = procCount;
            this.Expresion = expr;
            CodeLineIndex = codeLineIndex;
        }
        public int Execute(int id)
        {
            if(Gateway != null)
                pram.StartParallelExecution(Gateway.Read());
            else if(Expresion != null)
                pram.StartParallelExecution(Expresion.Result());
            else
                pram.StartParallelExecution(procCount);
            return InstructionPointer + 1;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            if (Gateway != null)
                return new Instr_ParallelStart(InstructionPointer, pram, Gateway, CodeLineIndex);
            else
                return new Instr_ParallelStart(InstructionPointer, pram, procCount, CodeLineIndex);
        }
    }
}
