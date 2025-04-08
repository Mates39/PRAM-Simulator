using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakalarka.Expresions;

namespace Bakalarka.Instructions
{
    internal class Instr_GoTo : IInstruction
    {
        public Instr_GoTo(int address, int codeLineIndex)
        {
            this.address = address;
            CodeLineIndex = codeLineIndex;
        }
        public Instr_GoTo(IExpresion expr, int codeLineIndex) { this.Expresion = expr; CodeLineIndex = codeLineIndex; }
        public int InstructionPointer { get; set; }
        public int CodeLineIndex { get; set; }
        public int address;
        public IExpresion Expresion;
        public int Execute(int procID)
        {
            if (Expresion != null)
                return Expresion.Result();
            return address;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_GoTo(address, CodeLineIndex);
        }
    }
}
