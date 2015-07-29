using System.Collections.Generic;
using System.Linq;

namespace Task3
{
    public class Analyzer
    {

        public readonly Dictionary<string, string> Variables = new Dictionary<string, string>();
        public readonly Dictionary<string, string> Functions = new Dictionary<string, string>();

        public bool TryAddVariable(string variableName, string variableValue)
        {
            if (Variables.ContainsKey(variableName)) return false;

            Variables[variableName] = variableValue;
            return true;
        }

        public bool TrySetVariable(string variableName, string variableValue)
        {
            if (!Variables.ContainsKey(variableValue))
                return false;
            Variables.Add(variableName, variableValue);
            return true;
        }
        public void ReplaceVaribales(Ast node)
        {
            if (Variables.ContainsKey(node.Value))
                node.Value = Variables[node.Value];
            foreach (var operand in node.Operands)
            {
                ReplaceVaribales(operand);
            }
        }
    }
}