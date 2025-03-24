using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakalarka.Expresions;

namespace Bakalarka.Instructions
{
    internal class Instr_WriteExprToPointerMultiMem : IInstruction
    {
        public int InstructionPointer { get; set; }
        public MemoryGateway Gateway { get; set; }
        public IExpresion ExprAddress { get; set; }
        public IExpresion ExprValue { get; set; }
        public PRAM pram { get; set; }
        public Instr_WriteExprToPointerMultiMem(MemoryGateway gat, int IP, IExpresion exprAddress, IExpresion exprValue)
        {
            InstructionPointer = IP;
            this.Gateway = gat;
            this.ExprAddress = exprAddress;
            this.ExprValue = exprValue;
        }
        public int Execute(int id)
        {
            Gateway.Write(ExprAddress.Result(), ExprValue.Result(), id);
            return InstructionPointer + 1;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_WriteExprToPointerMultiMem(Gateway.Duplicate(localGateway), InstructionPointer, ExprAddress.Duplicate(localGateway), ExprValue.Duplicate(localGateway));
        }
    }
}
