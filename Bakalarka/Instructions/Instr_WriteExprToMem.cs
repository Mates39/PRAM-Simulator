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

        public int Execute(int procID)
        {
            gateway.Write(expresion.Result(), procID);
            return InstructionPointer + 1;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_WriteExprToMem(gateway.Duplicate(localGateway), InstructionPointer, expresion.Duplicate(localGateway));
        }
    }
}
