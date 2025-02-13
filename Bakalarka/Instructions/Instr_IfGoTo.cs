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
        private IExpresion expresion;
        private int address;
        public int InstructionPointer { get; set; }
        public Instr_IfGoTo(IExpresion expr, int address, int IP) { expresion = expr; this.address = address; InstructionPointer = IP; }

        public int Execute()
        {
            if(expresion.Result() == 1)
            {
                return address;
            }
            else return InstructionPointer + 1;
        }
    }
}
