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
            if (op == Operator.DIV)
            {
                if (right.Read() == 0)
                    throw new Exception("Deleni nulou");
                return left.Read() / right.Read();
            }
            if(op == Operator.MOD) return left.Read()   % right.Read();
            else throw new Exception("expression fail");
        }
        public IExpresion Duplicate(LocalMemoryGateway localGateway)
        {
            return new Expr_CellCell(left.Duplicate(localGateway), op, right.Duplicate(localGateway));
        }
    }
}
