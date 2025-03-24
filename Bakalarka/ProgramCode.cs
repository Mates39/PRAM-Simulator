using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakalarka.Instructions;

namespace Bakalarka
{
    public class ProgramCode
    {
        public ProgramCode()
        {
        }
        public List<IInstruction> instructions = new List<IInstruction>();
        public ProgramCode Duplicate(LocalMemoryGateway lmg)
        {
            ProgramCode newProgram = new ProgramCode();
            foreach (IInstruction i in instructions)
            {
                newProgram.instructions.Add(i.Duplicate(lmg));
            }
            return newProgram;
        }
    }
}
