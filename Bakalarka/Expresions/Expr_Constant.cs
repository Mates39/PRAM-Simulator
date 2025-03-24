using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_Constant : IExpresion
    {
        private int value;
        public Expr_Constant(int value)
        {
            this.value = value;
        }
        public int Result()
        {
            return value;
        }
        public IExpresion Duplicate(LocalMemoryGateway localGateway)
        {
            return new Expr_Constant(value);
        }   
    }
}
