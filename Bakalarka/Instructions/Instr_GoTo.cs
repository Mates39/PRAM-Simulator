﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal class Instr_GoTo : IInstruction
    {
        public Instr_GoTo(int address) { this.address = address; }
        public int InstructionPointer { get; set; }
        public int address;
        public int Execute(int procID)
        {
            return address;
        }
        public IInstruction Duplicate(LocalMemoryGateway localGateway)
        {
            return new Instr_GoTo(address);
        }
    }
}
