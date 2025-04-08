using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bakalarka.Expresions;
using Bakalarka.Instructions;


namespace Bakalarka
{
    public class PRAM : INotifyPropertyChanged
    {
        private ObservableCollection<Processor> _processors;
        public ObservableCollection<Processor> Processors
        {
            get => _processors;
            set
            {
                _processors = value;
                OnPropertyChanged(nameof(Processors));
            }
        }
        public List<Processor> ActiveProcessors { get; set; }
        public ProgramCode MainProgram { get; set; }
        internal ProgramCode ParallelProgram { get; set; }
        public Dictionary<string, int> Jumps { get; set; }
        public int InstructionPointer { get; set; }
        public Memory InputMemory { get; set; }
        public Memory OutputMemory { get; set; }
        public Memory sharedMemory { get; set; }
        public CodeCompiler CodeCompiler { get; set; }
        public bool ParallelExecution { get; set; }
        public  bool Compiled { get; set; } 
        public int AccessType { get; set; } = PRAM_ACCESS_TYPE.CRCW_C;
        public SharedMemoryGateway sharedMemoryGateway { get; set; }
        public PRAM()
        {
            Processors = new ObservableCollection<Processor>();
            sharedMemory = new Memory();
            CodeCompiler = new CodeCompiler();
            MainProgram = new ProgramCode();
            ParallelProgram = new ProgramCode();
            Jumps = new Dictionary<string, int>();
            InputMemory = new Memory();
            OutputMemory = new Memory();
            sharedMemoryGateway = new SharedMemoryGateway(sharedMemory);
            ParallelExecution = false;
            InstructionPointer = 0;
            Compiled = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartParallelExecution(int numberOfProc)
        {
            if (numberOfProc > Processors.Count)
            {
                int count = numberOfProc - Processors.Count;
                for (int i = 0; i < count; i++)
                {
                    LocalMemoryGateway lmg = new LocalMemoryGateway();
                    Processor p = new Processor(Processors.Count, lmg);
                    lmg.ParralelProcIndex = p.Id;
                    p.Program = ParallelProgram.Duplicate(lmg);
                    Processors.Add(p);
                }
            }
            ActiveProcessors = Processors.ToList().GetRange(0,numberOfProc);
            foreach (Processor p in ActiveProcessors)
            {
                p.InstructionPointer = 0;
                p.Running = true;
            }
            ParallelExecution = true;
        }
        public void AddToSM()
        {
            sharedMemory.memory.Add(new MemCell(sharedMemory.memory.Count, 0));
        }
        public void RemoveFromSM()
        {
            if(sharedMemory.memory.Count > 0)
                sharedMemory.memory.RemoveAt(sharedMemory.memory.Count - 1);
        }

        public ObservableCollection<MemCell> GetSharedMemory()
        {
            return sharedMemory.memory;
        }
        public int ExecuteNextInstruction()
        {
            if (InstructionPointer >= 0)
            {
                InstructionPointer = this.MainProgram.instructions[InstructionPointer].Execute(0);
                if(sharedMemory.MemoryAccessCheck(AccessType))
                    return 1;
                else
                    return -1;
            }
            else
                return -2;
        }

        public int ExecuteNextParallelStep()
        {
            int halted = 0;
            foreach (Processor p in ActiveProcessors)
            {
                if (p.Running == true)
                {
                    p.ExecuteNextInstruction();
                    if (p.Running == false)
                        halted++;
                }
                else
                    halted++;
            }
            if (halted == ActiveProcessors.Count)
                ParallelExecution = false;
            if (sharedMemory.MemoryAccessCheck(AccessType))
                return 1;
            else
                return -1;
        }
        public void Restart()
        {
            InstructionPointer = 0;
            ParallelExecution = false;
            Processors.Clear();
            Processors.Add(new Processor(0, new LocalMemoryGateway()));
            Jumps.Clear();
            MainProgram.instructions.Clear();

        }
        public void Test()
        {
            string program1 = "S0 := S0 + 1\r\nparallelStart 2\r\nL1 := 3\r\nL1 := S0 + L1\r\nhalt\r\nparallelEnd\r\ngoto 0";
            string program2 = ":cycle\r\nS0 := S0 + 1\r\nif S0 > 3 goto :end\r\ngoto :cycle\r\n:end\r\nhalt";
            string program3 = ":cycle\r\nS[S2] := 69\r\nparallelStart 2\r\nL0 := {ind}\r\nS[L0 + 1] := 123\r\nhalt\r\nparallelEnd\r\nS2 := S2 + 1\r\nif S2 > 3 goto :end\r\ngoto :cycle\r\n:end\r\nhalt";
            string program4 = ":cycle\r\nS0 := S0 + 1\r\nif S0 > 3 goto :end\r\ngoto :cycle\r\n:end\r\nhalt";
            string program5 = "S4 := S5\r\n:cycle\r\nS3 := S4 % 2\r\nif S3 == 0 goto :preparePardo\r\nS4 := S4 + 1\r\n:preparePardo\r\nS4 := S4 / 2 \r\nparallelStart S4\r\nL0 := {ind}\r\nL1 := L0 * 2\r\nL1 := L1 + 6\r\nL2 := L1 + 1\r\nL3 := S[L1]\r\nL4 := S[L2] \r\nS[L1] := 0\r\nS[L2] := 0\r\nL5 := L3 + L4\r\nL1 := L1 - L0\r\nS[L1] := L5 \r\nhalt\r\nparallelEnd\r\nif S4 == 1 goto :end \r\ngoto :cycle \r\n:end\r\nhalt ";

            sharedMemory.memory.Add(new MemCell(0, 0));
            sharedMemory.memory.Add(new MemCell(1, 0));
            sharedMemory.memory.Add(new MemCell(2, 0));
            sharedMemory.memory.Add(new MemCell(3, 0));
            sharedMemory.memory.Add(new MemCell(4, 0));
            sharedMemory.memory.Add(new MemCell(5, 0));
            LocalMemoryGateway lmg = new LocalMemoryGateway();
            Processor p = new Processor(0, lmg);
            lmg.ParralelProcIndex = p.Id;
            Processors.Add(p);
            p.LocalMemory.memory.Add(new MemCell(0, 0));
            p.LocalMemory.memory.Add(new MemCell(1, 0));
            p.LocalMemory.memory.Add(new MemCell(2, 0));
            p.LocalMemory.memory.Add(new MemCell(3, 0));
            p.LocalMemory.memory.Add(new MemCell(4, 0));
            p.LocalMemory.memory.Add(new MemCell(5, 0));
            CodeCompiler.Compile(this, program5);
        }
    }
}
