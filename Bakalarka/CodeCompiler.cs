using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using Bakalarka.Expresions;
using Bakalarka.Instructions;
using System.Net.Http.Headers;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;

namespace Bakalarka
{
    public class CodeCompiler
    {
        public Regex comment = new Regex(@"^#(.+)$");
        public Regex emptyLine = new Regex(@"^\s*$");
        public Regex instr_WriteExprToMem = new Regex(@"^([SL]\d+)\s:=\s(.+)$");
        public Regex instr_WriteExprToPointer = new Regex(@"^([SL])\[(.+)]\s:=\s(.*)$");
        public Regex instr_ParallelStart = new Regex(@"^(parallelStart)\s(.+)$");
        public Regex instr_ParallelEnd = new Regex(@"^parallelEnd$");
        public Regex instr_IfGoTo = new Regex(@"^(if)\s(.+)\s(goto)\s(:[a-zA-Z]+)$");
        public Regex instr_GoTo = new Regex(@"^(goto)\s(:[a-zA-Z]+)$");
        public Regex instr_Halt = new Regex(@"^halt$");

        public Regex expr_Cell = new Regex(@"^[SL]\d+$");
        public Regex expr_CellCell = new Regex(@"^([SL]\d+)\s([\+\-\*\/\%])\s([SL]\d+)$");
        public Regex expr_CellConstant = new Regex(@"^([SL]\d+)\s([\+\-\*\/\%])\s(\d+)$");
        public Regex expr_Constant = new Regex(@"^\d+$");
        public Regex expr_Pointer = new Regex(@"^([SL])\[(.+)\]$");
        public Regex expr_ProcIndex = new Regex(@"^\{ind\}$");
        public Regex expr_Comparision = new Regex(@"^([SL]\d+)\s(==|!=|<=|>=|<|>)\s([SL]\d+|\d+)$");

        public Regex jump = new Regex(@"^(:[a-zA-Z]+)$");
        public CodeCompiler()
        {
        }
        public IExpresion IdentifyExpression(PRAM pram, LocalMemoryGateway lmg, string input)
        {
            Match match;
            if (expr_Cell.IsMatch(input))
            {
                match = expr_Cell.Match(input);
                if (match.Groups[0].Value[0] == 'S')
                {
                    return new Expr_Cell(new MemoryGateway(pram.sharedMemoryGateway, int.Parse(match.Groups[0].Value.Substring(1))));
                }
                else
                {
                    return new Expr_Cell(new MemoryGateway(lmg, int.Parse(match.Groups[0].Value.Substring(1))));
                }
            }
            else if (expr_CellCell.IsMatch(input))
            {
                match = expr_CellCell.Match(input);
                IGateway gateway1;
                IGateway gateway2;
                bool test = false;
                gateway1 = match.Groups[1].Value[0] == 'S' ? pram.sharedMemoryGateway : lmg;
                gateway2 = match.Groups[3].Value[0] == 'S' ? pram.sharedMemoryGateway : lmg;
                int gat1Index = int.Parse(match.Groups[1].Value.Substring(1));
                int gat2Index = int.Parse(match.Groups[3].Value.Substring(1));
                Operator op = match.Groups[2].Value == "+" ? Operator.ADD 
                    : match.Groups[2].Value == "-" ? Operator.SUB 
                    : match.Groups[2].Value == "*" ? Operator.MUL 
                    : match.Groups[2].Value == "%" ? Operator.MOD
                    : Operator.DIV;
                return new Expr_CellCell(new MemoryGateway(gateway1, gat1Index), op, new MemoryGateway(gateway2, gat2Index));
            }
            else if (expr_CellConstant.IsMatch(input))
            {
                match = expr_CellConstant.Match(input);
                IGateway gateway;
                gateway = match.Groups[1].Value[0] == 'S' ? pram.sharedMemoryGateway : lmg;
                int gatewayIndex = int.Parse(match.Groups[1].Value.Substring(1));
                Operator op = match.Groups[2].Value == "+" ? Operator.ADD
                    : match.Groups[2].Value == "-" ? Operator.SUB
                    : match.Groups[2].Value == "*" ? Operator.MUL
                    : match.Groups[2].Value == "%" ? Operator.MOD
                    : Operator.DIV;
                int value = int.Parse(match.Groups[3].Value);
                return new Expr_CellConstant(new MemoryGateway(gateway, gatewayIndex), op, value);
            }
            else if (expr_Constant.IsMatch(input))
            {
                match = expr_Constant.Match(input);
                return new Expr_Constant(int.Parse(match.Groups[0].Value));
            }
            else if (expr_Pointer.IsMatch(input))
            {
                match = expr_Pointer.Match(input);
                IGateway gateway1;
                IGateway gateway2;
                gateway1 = match.Groups[1].Value[0] == 'S' ? pram.sharedMemoryGateway : lmg;
                IExpresion expr = IdentifyExpression(pram, lmg, match.Groups[2].Value);
                //int gatewayIndex = int.Parse(match.Groups[2].Value.Substring(1));
                return new Expr_Pointer(new MemoryGateway(gateway1, 0), expr);
            }
            else if (expr_ProcIndex.IsMatch(input))
            {
                return new Expr_ProcIndex(lmg);
            }
            else if (expr_Comparision.IsMatch(input))
            {
                match = expr_Comparision.Match(input);
                IGateway gateway1;
                IGateway gateway2;
                gateway1 = match.Groups[1].Value[0] == 'S' ? pram.sharedMemoryGateway : lmg;
                int gatewayIndex1 = int.Parse(match.Groups[1].Value.Substring(1));
                Comparision comparision = match.Groups[2].Value == "==" ? Comparision.EQ
                    : match.Groups[2].Value == "!=" ? Comparision.NEQ
                    : match.Groups[2].Value == "<" ? Comparision.LT
                    : match.Groups[2].Value == ">" ? Comparision.GT
                    : match.Groups[2].Value == "<=" ? Comparision.LTE
                    : Comparision.GTE;
                if (match.Groups[3].Value[0] == 'S')
                {
                    gateway2 = pram.sharedMemoryGateway;
                    int gatewayIndex2 = int.Parse(match.Groups[3].Value.Substring(1));
                    return new Expr_Comparision(new MemoryGateway(gateway1, gatewayIndex1), comparision, new MemoryGateway(gateway2, gatewayIndex2));
                }
                else if(match.Groups[3].Value[0] == 'L')
                {
                    gateway2 = lmg;
                    int gatewayIndex2 = int.Parse(match.Groups[3].Value.Substring(1));
                    return new Expr_Comparision(new MemoryGateway(gateway1, gatewayIndex1), comparision, new MemoryGateway(gateway2, gatewayIndex2));
                }
                else
                {
                    return new Expr_Comparision(new MemoryGateway(gateway1, gatewayIndex1), comparision, int.Parse(match.Groups[3].Value));
                }
            }
            else
                throw new NotImplementedException();
        }
        public void Compile(PRAM pram, string input)
        {
            pram.Restart();
            Processor proc = pram.Processors[0];
            string[] lines = input.Split('\n');
            bool parallel = false;
            Match match;
            int instructionPointer = 0;
            int instructionPointerParallel = 0;
            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = lines[i].Trim();
                if (comment.IsMatch(lines[i]))
                {
                    continue;
                }
                if (emptyLine.IsMatch(lines[i]))
                {
                    continue;
                }
                if (instr_ParallelStart.IsMatch(lines[i]))
                {
                    parallel = true;
                    match = instr_ParallelStart.Match(lines[i]);
                    IExpresion expr = IdentifyExpression(pram, proc.Gateway, match.Groups[2].Value);
                    Instr_ParallelStart instruction = new Instr_ParallelStart(instructionPointer, pram, expr, i);
                    instruction.InstructionString = lines[i];
                    pram.MainProgram.instructions.Add(instruction);
                    instructionPointer++;
                }
                else if (instr_ParallelEnd.IsMatch(lines[i]))
                {
                    parallel = false;
                    match = instr_ParallelEnd.Match(lines[i]);
                    Instr_ParallelEnd instruction = new Instr_ParallelEnd(instructionPointer, pram, i);
                    instruction.InstructionString = lines[i];
                    pram.MainProgram.instructions.Add(instruction);
                    instructionPointer++;
                }
                else if (instr_GoTo.IsMatch(lines[i]))
                {
                    match = instr_GoTo.Match(lines[i]);
                    if (parallel)
                    {
                        //IExpresion expr = IdentifyExpression(pram, proc.Gateway, match.Groups[2].Value);
                        string jumpLabel = match.Groups[2].Value;
                        Expr_JumpLabel exprJump = new Expr_JumpLabel(jumpLabel, pram.Jumps);
                        Instr_GoTo instruction = new Instr_GoTo(exprJump, i);
                        instruction.InstructionString = lines[i];
                        proc.Program.instructions.Add(instruction);
                        instructionPointerParallel++;
                    }
                    else
                    {
                        //IExpresion expr = IdentifyExpression(pram, proc.Gateway, match.Groups[2].Value);
                        string jumpLabel = match.Groups[2].Value;
                        Expr_JumpLabel exprJump = new Expr_JumpLabel(jumpLabel, pram.Jumps);
                        Instr_GoTo instruction = new Instr_GoTo(exprJump, i);
                        instruction.InstructionString = lines[i];
                        pram.MainProgram.instructions.Add(instruction);
                        instructionPointer++;
                    }
                }
                else if (instr_Halt.IsMatch(lines[i]))
                {
                    match = instr_Halt.Match(lines[i]);
                    if (parallel)
                    {
                        Instr_Halt instruction = new Instr_Halt(instructionPointerParallel, i);
                        instruction.InstructionString = lines[i];
                        proc.Program.instructions.Add(instruction);
                        instructionPointerParallel++;
                    }
                    else
                    {
                        Instr_Halt instruction = new Instr_Halt(instructionPointer, i);
                        instruction.InstructionString = lines[i];
                        pram.MainProgram.instructions.Add(instruction);
                        instructionPointer++;
                    }
                }
                else if (instr_IfGoTo.IsMatch(lines[i]))
                {
                    match = instr_IfGoTo.Match(lines[i]);
                    if (parallel)
                    {
                        IExpresion expr = IdentifyExpression(pram, proc.Gateway, match.Groups[2].Value);
                        string jumpLabel = match.Groups[4].Value;
                        Expr_JumpLabel exprJump = new Expr_JumpLabel(jumpLabel, pram.Jumps);
                        Instr_IfGoTo instruction = new Instr_IfGoTo(expr, exprJump, instructionPointerParallel, i);
                        instruction.InstructionString = lines[i];
                        proc.Program.instructions.Add(instruction);
                        instructionPointerParallel++;
                    }
                    else
                    {
                        IExpresion expr = IdentifyExpression(pram, proc.Gateway, match.Groups[2].Value);
                        string jumpLabel = match.Groups[4].Value;
                        Expr_JumpLabel exprJump = new Expr_JumpLabel(jumpLabel, pram.Jumps);
                        Instr_IfGoTo instruction = new Instr_IfGoTo(expr, exprJump, instructionPointer, i);
                        instruction.InstructionString = lines[i];
                        pram.MainProgram.instructions.Add(instruction);
                        instructionPointer++;
                    }
                }
                else if (instr_WriteExprToMem.IsMatch(lines[i]))
                {
                    match = instr_WriteExprToMem.Match(lines[i]);
                    IGateway gateway;
                    if (match.Groups[1].Value[0] == 'L')
                        gateway = proc.Gateway;
                    else
                        gateway = pram.sharedMemoryGateway;
                    int gatewayIndex = int.Parse(match.Groups[1].Value.Substring(1));
                    IExpresion expr = IdentifyExpression(pram, proc.Gateway, match.Groups[2].Value);
                    if (parallel)
                    {
                        Instr_WriteExprToMem instruction = new Instr_WriteExprToMem(new MemoryGateway(gateway, gatewayIndex), instructionPointerParallel, expr, i);
                        instruction.InstructionString = lines[i];
                        proc.Program.instructions.Add(instruction);
                        instructionPointerParallel++;
                    }
                    else
                    {
                        Instr_WriteExprToMem instruction = new Instr_WriteExprToMem(new MemoryGateway(gateway, gatewayIndex), instructionPointer, expr, i);
                        instruction.InstructionString = lines[i];
                        pram.MainProgram.instructions.Add(instruction);
                        instructionPointer++;
                    }
                }
                else if (instr_WriteExprToPointer.IsMatch(lines[i]))
                {
                    match = instr_WriteExprToPointer.Match(lines[i]);
                    IGateway gateway;
                    if (match.Groups[1].Value[0] == 'L')
                        gateway = proc.Gateway;
                    else
                        gateway = pram.sharedMemoryGateway;
                    IExpresion exprAddress = IdentifyExpression(pram, proc.Gateway, match.Groups[2].Value);
                    IExpresion exprValue = IdentifyExpression(pram, proc.Gateway, match.Groups[3].Value);
                    if (parallel)
                    {
                        Instr_WriteExprToPointerMultiMem instruction = new Instr_WriteExprToPointerMultiMem(new MemoryGateway(gateway, 0), instructionPointerParallel, exprAddress, exprValue, i);
                        instruction.InstructionString = lines[i];
                        proc.Program.instructions.Add(instruction);
                        instructionPointerParallel++;
                    }
                    else
                    {
                        Instr_WriteExprToPointerMultiMem instruction = new Instr_WriteExprToPointerMultiMem(new MemoryGateway(gateway, 0), instructionPointer, exprAddress, exprValue, i);
                        instruction.InstructionString = lines[i];
                        pram.MainProgram.instructions.Add(instruction);
                        instructionPointer++;
                    }
                }
                else if (jump.IsMatch(lines[i]))
                {
                    if (parallel)
                    {
                        pram.Jumps.Add(lines[i], instructionPointerParallel);
                    }
                    else
                    {
                        pram.Jumps.Add(lines[i], instructionPointer);
                    }
                }
                else
                {
                    throw new Exception();
                    //pram.CompilationError = true;
                    //break;
                }
            }
            pram.ParallelProgram = proc.Program;
            pram.Compiled = true;
        }
    }
}
