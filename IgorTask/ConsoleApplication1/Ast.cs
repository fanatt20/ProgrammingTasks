using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Ast : IEquatable<Ast>
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

        public bool Equals(Ast other)
        {

            bool result = Value == other.Value;
            if (Operands.Count != other.Operands.Count)
                return false;

            for (int i = 0; i < Operands.Count; i++)
            {
                if (!Operands[i].Equals(other.Operands[i]))
                    return false;
            }

            return true;
        }
        public override string ToString()
        {
            var result = Value;
            
            foreach (var operand in Operands)
            {
                result += " "+operand;
            }
            return Value ;
        }
    }
}
