using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka
{
    public class Processor
    {
        public int Id { get; set; }
        public int InstructionPointer = 0;
        public bool Running = true;
        public int CurrentCodeLine {  get; set; }
        public ProgramCode Program { get; set; }
        public Memory LocalMemory { get; set; }
        public Dictionary<string, int> Jumps { get; set; }
        internal LocalMemoryGateway Gateway { get; set; }
        public Processor(int id, ProgramCode program, LocalMemoryGateway gateway)
        {
            this.Id = id;
            this.Program = program;
            this.LocalMemory = new Memory();
            this.Jumps = new Dictionary<string, int>();
            this.Gateway = gateway;
            Gateway.ParralelProcIndex = Id;
            Gateway.memory = LocalMemory;
        }
        public Processor(int id, LocalMemoryGateway gateway)
        {
            this.Id = id;
            this.LocalMemory = new Memory();
            this.Jumps = new Dictionary<string, int>();
            this.Program = new ProgramCode();
            Gateway = gateway;
            Gateway.ParralelProcIndex = Id;
            Gateway.memory = LocalMemory;
        }
        public void ExecuteNextInstruction()
        {
            if (InstructionPointer >= 0)
            {
                InstructionPointer = Program.instructions[InstructionPointer].Execute(Id);
                if (InstructionPointer >= 0)
                    CurrentCodeLine = Program.instructions[InstructionPointer].CodeLineIndex;
                else
                    CurrentCodeLine++;
                if (InstructionPointer < 0)
                    Running = false;
            }
        }

        public void Info()
        {
            Console.WriteLine("Processor ID: " + Id);
            Console.WriteLine("Instruction Pointer: " + InstructionPointer);
            if(InstructionPointer >= 0)
                Console.WriteLine("Instruction: " + Program.instructions[InstructionPointer]);
            else
                Console.WriteLine("Instruction: HALT");
            Console.Write("Local Memory: ");
            foreach (MemCell i in LocalMemory.memory)
            {
                Console.Write(i.Value + " ");
            }
            Console.WriteLine();

        }
    }
}
