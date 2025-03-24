using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_ProcIndex : IExpresion
    {
        private int ProcIndex;
        public Expr_ProcIndex(LocalMemoryGateway lmg)
        {
            ProcIndex = lmg.ParralelProcIndex;
        }

        public IExpresion Duplicate(LocalMemoryGateway localGateway)
        {
            return new Expr_ProcIndex(localGateway);
        }
        public int Result()
        {
            return ProcIndex;
        }
    }
}
