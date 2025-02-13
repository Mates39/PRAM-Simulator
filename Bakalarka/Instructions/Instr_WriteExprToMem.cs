using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakalarka.Expresions;

namespace Bakalarka.Instructions
{
    internal class Instr_WriteExprToMem : IInstruction
    {
        private MemoryGateway gateway;
        public int InstructionPointer { get; set; }
        public IExpresion expresion;
        public Instr_WriteExprToMem(MemoryGateway gat, int IP, IExpresion expr) { gateway = gat; InstructionPointer = IP; expresion = expr; }

        public int Execute()
        {
            gateway.Write(expresion.Result());
            return InstructionPointer + 1;
        }
    }
}
