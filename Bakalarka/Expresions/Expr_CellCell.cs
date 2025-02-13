using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_CellCell : IExpresion
    {
        private MemoryGateway left;
        private MemoryGateway right;
        private Operator op;
        public Expr_CellCell(MemoryGateway l, Operator op, MemoryGateway r) { this.left = l; this.op = op; this.right = r; }
        public int Result()
        {
            if(op == Operator.ADD) return left.Read() + right.Read();
            if (op == Operator.SUB) return left.Read() - right.Read();
            if (op == Operator.MUL) return left.Read() * right.Read();
            if (op == Operator.DIV) return left.Read() / right.Read();
            else throw new Exception("expression fail");
        }
    }
}
