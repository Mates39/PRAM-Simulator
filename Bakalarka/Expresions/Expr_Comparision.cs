using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_Comparision : IExpresion
    {
        private MemoryGateway left;
        private MemoryGateway? rightCell;
        private int? rightValue;
        private Comparision comparision;
        public Expr_Comparision(MemoryGateway l, Comparision c, MemoryGateway r) { this.left = l; this.comparision = c; this.rightCell = r; }
        public Expr_Comparision(MemoryGateway l, Comparision c, int r) { this.left = l; this.comparision = c; this.rightValue = r; }

        public int Result()
        {
            if(rightCell != null)
            {
                if(comparision == Comparision.EQ) { return left.Read() == rightCell.Read() ? 1 : 0; }
                if (comparision == Comparision.NEQ) { return left.Read() != rightCell.Read() ? 1 : 0; }
                if (comparision == Comparision.GT) { return left.Read() > rightCell.Read() ? 1 : 0; }
                if (comparision == Comparision.LT) { return left.Read() < rightCell.Read() ? 1 : 0; }
                if (comparision == Comparision.GTE) { return left.Read() >= rightCell.Read() ? 1 : 0; }
                if (comparision == Comparision.LTE) { return left.Read() <= rightCell.Read() ? 1 : 0; }
                throw new Exception("Unknown comparision");
            }
            else
            {
                if (comparision == Comparision.EQ) { return left.Read() == rightValue ? 1 : 0; }
                if (comparision == Comparision.NEQ) { return left.Read() != rightValue ? 1 : 0; }
                if (comparision == Comparision.GT) { return left.Read() > rightValue ? 1 : 0; }
                if (comparision == Comparision.LT) { return left.Read() < rightValue ? 1 : 0; }
                if (comparision == Comparision.GTE) { return left.Read() >= rightValue ? 1 : 0; }
                if (comparision == Comparision.LTE) { return left.Read() <= rightValue ? 1 : 0; }
                throw new Exception("Unknown comparision");
            }
        }
    }
}
