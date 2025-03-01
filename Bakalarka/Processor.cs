using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    internal class Processor
    {
        public int id;
        public int InstructionPointer = 0;
        public ProgramCode program;
        public Memory localMemory;
        public IGateway gateway;
        public Processor(int id, ProgramCode program, IGateway gateway)
        {
            this.id = id;
            this.program = program;
            localMemory = new Memory();
            this.gateway = gateway;

            gateway.memory = localMemory;
        }
        public void ExecuteNextInstruction()
        {
            if (InstructionPointer >= 0)
            {
                InstructionPointer = program.instructions[InstructionPointer].Execute(id);
            }
        }

        public void Info()
        {
            Console.WriteLine("Processor ID: " + id);
            Console.WriteLine("Instruction Pointer: " + InstructionPointer);
            if(InstructionPointer >= 0)
                Console.WriteLine("Instruction: " + program.instructions[InstructionPointer]);
            else
                Console.WriteLine("Instruction: HALT");
            Console.Write("Local Memory: ");
            foreach (int i in localMemory.memory)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

        }
    }
}
