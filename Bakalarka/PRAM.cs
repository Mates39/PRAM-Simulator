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
using Microsoft.VisualBasic;


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
        public int InstructionCounter { get; set; }
        public int AccessType { get; set; } = PRAM_ACCESS_TYPE.CRCW_C;
        public int CurrentCodeLine { get; set; }
        public string CodeString { get; set; }
        public string MemoryInputString { get; set; }
        public IBrowserFile? UploadedCode;
        public SharedMemoryGateway sharedMemoryGateway { get; set; }
        public bool MemoryAccessError { get; set; }
        public bool CompilationError { get; set; }
        public bool LoadingFileError { get; set; }
        public bool Halted { get; set; }
        public bool MemoryIndexError {  get; set; }
        public string ErrorMessage { get; set; }
        public int Breakpoint { get; set; }
        public bool AutoRun { get; set; }
        public HashSet<int> Breakpoints { get; set; }
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
            MemoryInputString = "";
            MemoryAccessError = false;
            CompilationError = false;
            MemoryIndexError = false;
            Halted = false;
            Breakpoints = new HashSet<int>();
            InstructionCounter = 0;
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
                try
                {
                    InstructionPointer = this.MainProgram.instructions[InstructionPointer].Execute(0);
                    InstructionCounter++;
                }
                catch (Exception e)
                {
                    MemoryIndexError = true;
                    ErrorMessage = e.Message;
                }
                try
                {
                    sharedMemory.MemoryAccessCheck(AccessType);
                    return 1;
                }
                catch(Exception e)
                {
                    return -1;
                }

            }
            else
            {
                Halted = true;
                return -2;
            }
        }

        public async void ExecuteNextParallelStep()
        {
            int halted = 0;
            foreach (Processor p in ActiveProcessors)
            {
                if (p.Running == true)
                {
                    try
                    {
                        p.ExecuteNextInstruction();
                        InstructionCounter++;
                        if (p.Running == false)
                            halted++;
                    }
                    catch(Exception e)
                    {
                        MemoryIndexError = true;
                        ErrorMessage = e.Message;
                    }
                }
                else
                    halted++;
            }
            if (halted == ActiveProcessors.Count)
                ParallelExecution = false;
            try
            {
                await sharedMemory.MemoryAccessCheck(AccessType);
                //return 1;
            }
            catch(Exception)
            {
                MemoryAccessError = true;
            }
            //return 1;
        }
        public void Restart()
        {
            Compiled = false;
            Halted = false;
            InstructionPointer = 0;
            CurrentCodeLine = -1;
            ParallelExecution = false;
            Processors.Clear();
            Processors.Add(new Processor(0, new LocalMemoryGateway()));
            Jumps.Clear();
            MainProgram.instructions.Clear();
            ClearErrors();
            InstructionCounter = 0;
        }
        public void Reset()
        {
            Halted = false;
            InstructionPointer = 0;
            CurrentCodeLine = -1;
            ParallelExecution = false;
            ClearErrors();
            InstructionCounter = 0;
        }
        private void ClearErrors()
        {
            MemoryAccessError = false;
            CompilationError = false;
            MemoryIndexError = false;
        }
    }
}
