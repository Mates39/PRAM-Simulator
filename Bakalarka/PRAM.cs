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
using Microsoft.AspNetCore.Components.Forms;


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
        public int CurrentCodeLine { get; set; }
        public string CodeString { get; set; }
        public IBrowserFile? UploadedCode;
        public SharedMemoryGateway sharedMemoryGateway { get; set; }
        public bool MemoryAccessError { get; set; }
        public bool CompilationError { get; set; }
        public bool Halted { get; set; }
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
            CodeString = ""; 
            MemoryAccessError = false;
            CompilationError = false;
            Halted = false;
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
                p.CurrentCodeLine = p.Program.instructions[p.InstructionPointer].CodeLineIndex;
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
                if (sharedMemory.MemoryAccessCheck(AccessType))
                    return 1;
                else
                    return -1;
            }
            else
            {
                Halted = true;
                return -2;
            }
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
            {
                MemoryAccessError = true;
                return -1;
            }
        }
        public void Restart()
        {
            Compiled = false;
            Halted = false;
            InstructionPointer = 0;
            CurrentCodeLine = 1000;
            ParallelExecution = false;
            Processors.Clear();
            Processors.Add(new Processor(0, new LocalMemoryGateway()));
            Processors[0].LocalMemory.memory.Add(new MemCell(0, 0));
            Processors[0].LocalMemory.memory.Add(new MemCell(1, 0));
            Jumps.Clear();
            MainProgram.instructions.Clear();
            ClearErrors();

        }
        private void ClearErrors()
        {
            MemoryAccessError = false;
            CompilationError = false;
        }
    }
}
