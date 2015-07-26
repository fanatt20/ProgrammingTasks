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
        public List<string> ReplaceVaribales(string[] tokens)
        {
            var tokenList = tokens.ToList();
            for (int i = 0; i < tokenList.Count; i++)
            {
                if (!Variables.ContainsKey(tokenList[i])) continue;
                tokenList.Insert(i, Variables[tokenList[i]]);
                tokenList.Remove(tokenList[i + 1]);
            }
            return tokenList;
        }
    }
}