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
        private Analyzer _analyzer = new Analyzer();



        public string Parse(string input)
        {
            var tokens = _tokenizer.GetTokens(input);
            var result = string.Empty;
            if (IsExpresion(tokens))
            {
                var tokenList = _analyzer.ReplaceVaribales(tokens);
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
            if (_analyzer.Variables.ContainsKey(funcName))
                return "Same name as variable";

            var result = string.Empty;
            var variableValue = tokens[3];


            if (_analyzer.Variables.ContainsKey(funcName))
            {
                _analyzer.Functions[funcName] = variableValue;
                result = "Function Changed";
            }
            else
            {
                _analyzer.Functions.Add(funcName, variableValue);
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
            if (_analyzer.Functions.ContainsKey(variableName))
                return "Same name as function";

            var result = string.Empty;
            var variableValue = tokens[3];

            if (tokens[3] == "(")
            {
                var subExpr = tokens.Select(str => str).Where((value, index) => index > 2 && index < tokens.Length - 1).ToArray();
                variableValue = _solver.Solve(_builder.Build(subExpr)).ToString();
            }

            if (_analyzer.TrySetVariable(variableName, variableValue))
            {
                result = "Variable Changed";
            }
            else
            {
                if (_analyzer.TryAddVariable(variableName, variableValue))
                    result = "Variable Created";
                else
                    result = "Errore";


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
