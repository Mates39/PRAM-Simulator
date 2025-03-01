using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class PRAM
    {
        public List<Processor> processors { get; set; }
        public ProgramCode program { get; set; }
        public Memory sharedMemory { get; set; }
        public PRAM()
        {
            processors = new List<Processor>();
            sharedMemory = new Memory();
            program = new ProgramCode();
        }
        public void StartParallelExecution(int numberOfProc)
        {

        }

        public void ExecuteNextParallelStep()
        {
            int count = 0;
            foreach (Processor p in processors)
            {
                if (p.InstructionPointer >= 0)
                {
                    p.ExecuteNextInstruction();
                    count++;
                }
            }
           Console.WriteLine("executed " + count + " instructions");
        }

        public void Info()
        {
            Console.Write("Shared Memory: ");
            foreach (int i in sharedMemory.memory)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            foreach (Processor p in processors)
            {
                p.Info();
            }
            Console.WriteLine();
        }
    }
}
