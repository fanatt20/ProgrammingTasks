using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Interpreter
    {

        private AstBuilder _builder = new AstBuilder();
        private AstSolver _solver = new AstSolver();
        private Tokenizer _tokenizer = new Tokenizer();
        private Dictionary<string, string> _variables = new Dictionary<string, string>();
        private Dictionary<string, string> _functions = new Dictionary<string, string>();


        public string Parse(string input)
        {
            var tokens = _tokenizer.GetTokens(input);
            var result = string.Empty;
            if (IsExpresion(tokens))
            {
                var tokenList = tokens.ToList();
                for (int i = 0; i < tokenList.Count; i++)
                {
                    if (!_variables.ContainsKey(tokenList[i])) continue;
                    tokenList.Insert(i, _variables[tokenList[i]]);
                    tokenList.Remove(tokenList[i+1]);
                }
                result = _solver.Solve(_builder.Build(tokenList.ToArray())).ToString();
            }
            else if (IsVariableDeclaration(tokens))
                result = DeclareVariable(tokens);
            else if (IsFuncDeclare(tokens))
                result = DeclareFunc(tokens);

            return result;
        }

        private string DeclareFunc(string[] tokens)
        {
            var funcName = tokens[3];
            if (_variables.ContainsKey(funcName))
                return "Same name as variable";

            var result = string.Empty;
            var variableValue = tokens[3];


            if (_variables.ContainsKey(funcName))
            {
                _functions[funcName] = variableValue;
                result = "Function Changed";
            }
            else
            {
                _functions.Add(funcName, variableValue);
                result = "Function Created";
            }

            return result;
        }

        private bool IsFuncDeclare(string[] tokens)
        {
            return tokens[1] == "def" && tokens[2] == "(";
        }

        private string DeclareVariable(string[] tokens)
        {
            var variableName = tokens[2];
            if (_functions.ContainsKey(variableName))
                return "Same name as func";

            var result = string.Empty;
            var variableValue = tokens[3];
            
            if (tokens[3] == "(")
            {
                var subExpr = tokens.Select(str => str).Where((value, index) => index > 2 && index < tokens.Length - 1).ToArray();
                variableValue = _solver.Solve(_builder.Build(subExpr)).ToString();
            }

            if (_variables.ContainsKey(variableName))
            {
                _variables[variableName] = variableValue;
                result = "Variable Changed";
            }
            else
            {
                _variables.Add(variableName, variableValue);
                result = "Variable Created";
            }

            return result;
        }

        private bool IsVariableDeclaration(string[] tokens)
        {
            return tokens[1] == "def" && tokens[2] != "(";
        }

        private bool IsExpresion(string[] tokens)
        {
            return tokens[1] != "def";

        }

    }
}
