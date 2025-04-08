using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakalarka.Expresions;

namespace Bakalarka.Instructions
{
    internal class Instr_IfGoTo : IInstruction
    {
        private IExpresion ExprCondition { get; set; }
        private IExpresion ExprAddress { get; set; }
        private int Address;
        public int InstructionPointer { get; set; }
        public int CodeLineIndex { get; set; }
        public Instr_IfGoTo(IExpresion exprCondition, IExpresion exprAddress, int IP, int codeLineIndex) { ExprCondition = exprCondition; ExprAddress = exprAddress; InstructionPointer = IP; CodeLineIndex = codeLineIndex; }

        public int Execute(int procID)
        {
            if(ExprCondition.Result() == 1)
            {
                return ExprAddress.Result();
            }
            else return InstructionPointer + 1;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_IfGoTo(ExprCondition.Duplicate(localGateway), ExprAddress.Duplicate(localGateway), InstructionPointer, CodeLineIndex);
        }
    }
}
