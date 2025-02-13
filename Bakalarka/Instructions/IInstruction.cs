using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    internal interface IInstruction
    {
        int InstructionPointer { get; set; }
        int Execute();
    }
}
