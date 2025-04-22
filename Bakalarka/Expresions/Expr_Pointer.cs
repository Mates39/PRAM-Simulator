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
        //private Expr_Cell cell;
        private IExpresion cell;
        //public Expr_Pointer(MemoryGateway gat, Expr_Cell cell) { this.gateway = gat; this.cell = cell; }
        public Expr_Pointer(MemoryGateway gat, IExpresion cell) { this.gateway = gat; this.cell = cell; }
        public int Result()
        { 
            return gateway.Read(cell.Result());
        }
        public IExpresion Duplicate(LocalMemoryGateway localGateway)
        {
            return new Expr_Pointer(gateway.Duplicate(localGateway), (IExpresion)cell.Duplicate(localGateway));
        }
    }
}
