using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class PRAM
    {
        public List<Processor> processors = new List<Processor>();
        public ProgramCode program;
        public List<int> sharedMemory;
        public void StartParallelExecution(int numberOfProc)
        {
            
        }

        public void ExecuteNextParallelStep()
        {
            foreach(Processor p in processors)
            {
                p.ExecuteNextInstruction();
            }
        }
    }
}
