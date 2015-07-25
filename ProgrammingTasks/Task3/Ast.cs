using System;
using System.Collections.Generic;

namespace Task3
{
    public class Ast : IEquatable<Ast>
    {
        public Ast(string value, bool isOperator = false)
        {
            Operands = new List<Ast>();
            Value = value;
            IsOperator = isOperator;
        }

        public string Value { get; private set; }
        public List<Ast> Operands { get; private set; }
        public bool IsOperator { get; private set; }

        public bool Equals(Ast other)
        {
            var result = Value == other.Value;
            if (Operands.Count != other.Operands.Count)
                return false;

            for (var i = 0; i < Operands.Count; i++)
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
                result += " " + operand;
            }
            return Value;
        }
    }
}