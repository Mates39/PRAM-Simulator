using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_Cell : IExpresion
    {
        MemoryGateway gateway;
        public Expr_Cell(MemoryGateway gat) { this.gateway = gat; }
        public int Result() { return gateway.Read(); }
    }
}
