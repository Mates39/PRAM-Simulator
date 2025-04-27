using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using Bakalarka.Expresions;
using Bakalarka.Instructions;

namespace Bakalarka
{
    //Regex instr_WriteExprToMem = new Regex(@"^([SL]\d+)\s:=\s(.+)$");
    //Regex instr_WriteExprToPointer = new Regex(@"^([SL])\[(.+\)]\s:=\s(.*)$");
    //Regex instr_ParallelStart = new Regex(@"^(parallelStart)\s(.+)$");
    //Regex instr_ParallelEnd = new Regex(@"^(parallelEnd)\s(.+)$");
    //Regex instr_IfGoTo = new Regex(@"^(if)\s(.+)\s(goto)\s(.+)$");
    //Regex instr_GoTo = new Regex(@"^(goto)\s(.+)$");
    //Regex instr_Halt = new Regex(@"^halt$");

    //Regex expr_Cell = new Regex(@"^[SL]\d+$");
    //Regex expr_CellCell = new Regex(@"^([SL]\d+)\s([\+\-\*\/])\s([SL]\d+)$");
    //Regex expr_CellConstant = new Regex(@"^([SL]\d+)\s([\+\-\*\/])\s(\d+)$");
    //Regex expr_Constant = new Regex(@"^\d+$");
    //Regex expr_Pointer = new Regex(@"^([SL])\[([SL]\d+)\]$");
    //Regex expr_ProcIndex = new Regex(@"^\{ind\}$");
    //Regex expr_Comparision = new Regex(@"^([SL]\d+)\s(==|!=|<=|>=|<|>)\s(([SL]\d+)|(\d+))$");



    static class PRAM_ACCESS_TYPE
    {
        public const int CRCW_C = 0;
        public const int CRCW_A = 1;
        public const int CRCW_P = 2;
        public const int CREW = 3;
        public const int EREW = 4;
    }
    class Program
    {
        static void Main(string[] args)
        {

            Regex reg = new Regex(@"^\((\d+):(\d+)\)$");
            Console.WriteLine(reg.IsMatch("(1:0)"));
        }

    }
}

