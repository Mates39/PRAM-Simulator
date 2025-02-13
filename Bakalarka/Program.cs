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
            List<int> sm = new List<int>();
            sm.Add(2);
            sm.Add(18);
            sm.Add(5);
            sm.Add(12);
            sm.Add(21);
            sm.Add(8);
            sm.Add(10);
            sm.Add(7);
            ProgramCode program1 = new ProgramCode();
            ProgramCode program2 = new ProgramCode();
            ProgramCode program3 = new ProgramCode();
            ProgramCode program4 = new ProgramCode();
            program1.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sm, 0), 0, new Expr_CellCell(new MemoryGateway(sm, 0), Operator.ADD, new MemoryGateway(sm, 1))));
            program1.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sm, 0), 1, new Expr_CellCell(new MemoryGateway(sm, 0), Operator.ADD, new MemoryGateway(sm, 2))));
            program1.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sm, 0), 2, new Expr_CellCell(new MemoryGateway(sm, 0), Operator.ADD, new MemoryGateway(sm, 4))));
            program1.instructions.Add(new Instr_Halt(3));
            program2.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sm, 2), 0, new Expr_CellCell(new MemoryGateway(sm, 2), Operator.ADD, new MemoryGateway(sm, 3))));
            program2.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sm, 4), 1, new Expr_CellCell(new MemoryGateway(sm, 4), Operator.ADD, new MemoryGateway(sm, 6))));
            program2.instructions.Add(new Instr_NoOp(2));
            program2.instructions.Add(new Instr_Halt(3));
            program3.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sm, 4), 0, new Expr_CellCell(new MemoryGateway(sm, 4), Operator.ADD, new MemoryGateway(sm, 5))));
            program3.instructions.Add(new Instr_NoOp(1));
            program3.instructions.Add(new Instr_NoOp(2));
            program3.instructions.Add(new Instr_Halt(3));
            program4.instructions.Add(new Instr_WriteExprToMem(new MemoryGateway(sm, 6), 0, new Expr_CellCell(new MemoryGateway(sm, 6), Operator.ADD, new MemoryGateway(sm, 7))));
            program4.instructions.Add(new Instr_NoOp(1));
            program4.instructions.Add(new Instr_NoOp(2));
            program4.instructions.Add(new Instr_Halt(3));
            Processor p1 = new Processor(0, program1);
            Processor p2 = new Processor(1, program2);
            Processor p3 = new Processor(2, program3);
            Processor p4 = new Processor(3, program4);
            PRAM pram = new PRAM();
            pram.processors.Add(p1);
            pram.processors.Add(p2);
            pram.processors.Add(p3);
            pram.processors.Add(p4);
            pram.ExecuteNextParallelStep();
            pram.ExecuteNextParallelStep();
            pram.ExecuteNextParallelStep();
            pram.ExecuteNextParallelStep();
            pram.ExecuteNextParallelStep();

            Console.WriteLine(sm.ElementAt(0));


        }

    }
}