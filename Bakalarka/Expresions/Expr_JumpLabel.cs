using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    internal class Expr_JumpLabel : IExpresion
    {
        public string Label { get; set; }
        public Dictionary<string, int> Jumps { get; set; }
        public Expr_JumpLabel(string label, Dictionary<string, int> jumps) 
        {
            Label = label;
            Jumps = jumps;
        }

        public IExpresion Duplicate(LocalMemoryGateway localGateway)
        {
            return new Expr_JumpLabel(Label, Jumps);
        }

        public int Result()
        {
            return Jumps[Label];
        }
    }
}
