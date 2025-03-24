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
        internal ProgramCode MainProgram { get; set; }
        internal ProgramCode ParallelProgram { get; set; }
        internal int InstructionPointer { get; set; }
        public Memory InputMemory { get; set; }
        public Memory OutputMemory { get; set; }
        public Memory sharedMemory { get; set; }
        public bool ParallelExecution { get; set; }
        public int AccessType { get; set; } = PRAM_ACCESS_TYPE.CRCW_C;
        public SharedMemoryGateway sharedMemoryGateway { get; set; }
        public PRAM()
        {
            Processors = new ObservableCollection<Processor>();
            sharedMemory = new Memory();
            MainProgram = new ProgramCode();
            InputMemory = new Memory();
            OutputMemory = new Memory();
            sharedMemoryGateway = new SharedMemoryGateway(sharedMemory);
            ParallelExecution = false;
            InstructionPointer = 0;
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
                    p.LocalMemory.memory.Add(new MemCell(0, 0));
                    p.LocalMemory.memory.Add(new MemCell(1, 0));
                    p.LocalMemory.memory.Add(new MemCell(2, 0));
                    p.LocalMemory.memory.Add(new MemCell(3, 0));
                    p.LocalMemory.memory.Add(new MemCell(4, 0));
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

        public void Run()
        {
            ProgramCode masterProgram = new ProgramCode();
            masterProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sharedMemoryGateway, 0), 0, new Expr_Constant(8)));
            masterProgram.instructions.Add(new Instr_ParallelStart(1, this, 4));
            masterProgram.instructions.Add(new Instr_ParallelEnd(2, this));
            masterProgram.instructions.Add(new Instr_GoTo(0));
            this.MainProgram = masterProgram;
            this.sharedMemory.memory.Add(new MemCell(0, 1));
            for (int i = 0; i < 4; i++)
            {
                LocalMemoryGateway lmg = new LocalMemoryGateway();
                Processor p = new Processor(i, lmg);
                lmg.ParralelProcIndex = p.Id;
                ProgramCode program = new ProgramCode();
                program.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sharedMemoryGateway, 0), 0, new Expr_Cell(new MemoryGateway(lmg, 0))));
                program.instructions.Add(new Instr_Halt(1));
                p.Program = program;
                p.LocalMemory.memory.Add(new MemCell(0, p.Id+10));
                Processors.Add(p);
            }


        }

        public void Program_Sum()
        {
            ProgramCode mainProgram = new ProgramCode();
            mainProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sharedMemoryGateway, 0), 0, new Expr_CellConstant(new MemoryGateway(sharedMemoryGateway, 0), Operator.DIV, 2)));
            mainProgram.instructions.Add(new Instr_ParallelStart(1, this, new MemoryGateway(sharedMemoryGateway, 0)));
            mainProgram.instructions.Add(new Instr_ParallelEnd(2, this));
            mainProgram.instructions.Add(new Instr_IfGoTo(new Expr_Comparision(new MemoryGateway(sharedMemoryGateway, 0), Comparision.EQ, 1), 5, 3));
            mainProgram.instructions.Add(new Instr_GoTo(0));
            mainProgram.instructions.Add(new Instr_Halt(5));
            this.MainProgram = mainProgram;
            this.sharedMemory.memory.Add(new MemCell(0, 8));
            this.sharedMemory.memory.Add(new MemCell(1, 2));
            this.sharedMemory.memory.Add(new MemCell(2, 18));
            this.sharedMemory.memory.Add(new MemCell(3, 5));
            this.sharedMemory.memory.Add(new MemCell(4, 12));
            this.sharedMemory.memory.Add(new MemCell(5, 21));
            this.sharedMemory.memory.Add(new MemCell(6, 8));
            this.sharedMemory.memory.Add(new MemCell(7, 10));
            this.sharedMemory.memory.Add(new MemCell(8, 7));

            ProgramCode parallelProgram = new ProgramCode();

            LocalMemoryGateway lmg = new LocalMemoryGateway();
            Processor p = new Processor(0, lmg);
            lmg.ParralelProcIndex = p.Id;
            parallelProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(lmg, 0), 0, new Expr_ProcIndex(lmg)));
            parallelProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(lmg, 1), 1, new Expr_CellConstant(new MemoryGateway(lmg, 0), Operator.MUL, 2)));
            parallelProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(lmg, 1), 2, new Expr_CellConstant(new MemoryGateway(lmg, 1), Operator.ADD, 1)));
            parallelProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(lmg, 2), 3, new Expr_CellConstant(new MemoryGateway(lmg, 1), Operator.ADD, 1)));
            parallelProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(lmg, 3), 4, new Expr_Pointer(new MemoryGateway(sharedMemoryGateway, 0), new Expr_Cell(new MemoryGateway(lmg, 1)))));
            parallelProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(lmg, 4), 5, new Expr_Pointer(new MemoryGateway(sharedMemoryGateway, 0), new Expr_Cell(new MemoryGateway(lmg, 2)))));
            parallelProgram.instructions.Add(new Instr_WriteExprToPointerMultiMem(new MemoryGateway(sharedMemoryGateway, 0), 6, new Expr_Cell(new MemoryGateway(lmg, 1)), new Expr_Constant(0)));
            parallelProgram.instructions.Add(new Instr_WriteExprToPointerMultiMem(new MemoryGateway(sharedMemoryGateway, 0), 7, new Expr_Cell(new MemoryGateway(lmg, 2)), new Expr_Constant(0)));
            parallelProgram.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(lmg, 1), 8, new Expr_CellCell(new MemoryGateway(lmg, 1), Operator.SUB, new MemoryGateway(lmg, 0))));
            parallelProgram.instructions.Add(new Instr_WriteExprToPointerMultiMem(new MemoryGateway(sharedMemoryGateway, 0), 9, new Expr_Cell(new MemoryGateway(lmg, 1)), new Expr_CellCell(new MemoryGateway(lmg, 3), Operator.ADD, new MemoryGateway(lmg, 4))));
            parallelProgram.instructions.Add(new Instr_Halt(10));
            p.Program = parallelProgram;
            p.LocalMemory.memory.Add(new MemCell(0, 0));
            p.LocalMemory.memory.Add(new MemCell(1, 0));
            p.LocalMemory.memory.Add(new MemCell(2, 0));
            p.LocalMemory.memory.Add(new MemCell(3, 0));
            p.LocalMemory.memory.Add(new MemCell(4, 0));
            Processors.Add(p);
            this.ParallelProgram = parallelProgram;
        }
    }
}
