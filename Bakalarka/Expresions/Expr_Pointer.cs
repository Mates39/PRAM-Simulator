using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_Pointer : IExpresion
    {
        private MemoryGateway gateway;
        public Expr_Pointer(MemoryGateway gat) { this.gateway = gat; }
        public int Result()
        { 
            return gateway.Read(gateway.Read());
        }
    }
}
