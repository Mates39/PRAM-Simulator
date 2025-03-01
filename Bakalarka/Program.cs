using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;
using Bakalarka.Expresions;
using Bakalarka.Instructions;

namespace Bakalarka
{

    class Program
    {
        static void Main(string[] args)
        {
            SharedMemoryGateway smg = new SharedMemoryGateway();
            LocalMemoryGateway lmg1 = new LocalMemoryGateway();
            LocalMemoryGateway lmg2 = new LocalMemoryGateway();

            PRAM pram = new PRAM();


            ProgramCode program = new ProgramCode();
            program.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(smg, 0), 0, new Expr_CellConstant(new MemoryGateway(smg, 0), Operator.MUL, 2)));
            program.instructions.Add(new Instr_IfGoTo(new Expr_Comparision(new MemoryGateway(smg, 0), Comparision.LT, 4), 0, 1));
            program.instructions.Add(new Instr_Halt(2));

            ProgramCode program2 = new ProgramCode();
            program2.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(smg, 1), 0, new Expr_CellConstant(new MemoryGateway(smg, 1), Operator.MUL, 2)));
            program2.instructions.Add(new Instr_IfGoTo(new Expr_Comparision(new MemoryGateway(smg, 1), Comparision.LT, 96), 0, 1));
            program2.instructions.Add(new Instr_Halt(2));

            Processor p1 = new Processor(0, program, lmg1);
            Processor p2 = new Processor(1, program2, lmg2);

            smg.memory = pram.sharedMemory;
            pram.sharedMemory.memory = new List<int>() {1,3 };
            p1.localMemory.memory.Add(1);
            p2.localMemory.memory.Add(1);

            pram.processors.Add(p1);
            pram.processors.Add(p2);
            pram.Info();
            pram.ExecuteNextParallelStep();
            pram.Info();
            Console.ReadLine();
            pram.ExecuteNextParallelStep();
            pram.Info();
            Console.ReadLine();
            pram.ExecuteNextParallelStep();
            pram.Info();
            Console.ReadLine();
            pram.ExecuteNextParallelStep();
            pram.Info();
            Console.ReadLine();
            pram.ExecuteNextParallelStep();
            pram.Info();
            Console.ReadLine();







        }

    }
}