using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    class Ast
    {

        public string Value { get; private set; }
        public List<Ast> Operands { get; private set; }

        public bool IsOperator { get; private set; }

        public Ast(string value, bool isOperator = false)
        {
            Operands = new List<Ast>();
            Value = value;
            IsOperator = isOperator;

        }
    }
}
