using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_CellConstant : IExpresion
    {
        private MemoryGateway gateway;
        private Operator op;
        private int value;
        public Expr_CellConstant(MemoryGateway gat, Operator op,  int val) { this.gateway = gat; this.op = op; this.value = val; }
        public int Result()
        {
            if (op == Operator.ADD) return gateway.Read() + value;
            if (op == Operator.SUB) return gateway.Read() - value;
            if (op == Operator.MUL) return gateway.Read() * value;
            if (op == Operator.DIV) return gateway.Read() / value;
            else throw new Exception("expression fail");
        }
    }
}
