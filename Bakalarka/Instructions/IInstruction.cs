using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Instructions
{
    public interface IInstruction
    {
        int InstructionPointer { get; set; }
        int CodeLineIndex { get; set; }
        int Execute(int procID);
        IInstruction Duplicate(LocalMemoryGateway localGateway);
    }
}
