using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakalarka.Expresions
{
    public enum Operator
    {
        ADD,
        SUB,
        MUL,
        DIV
    }
    public enum Comparision
    {
        EQ,
        NEQ,
        GT,
        LT,
        GTE,
        LTE
    }
    public interface IExpresion
    {
        int Result();
    }
}
