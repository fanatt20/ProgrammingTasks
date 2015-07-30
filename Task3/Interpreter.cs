using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Interpreter
    {
        private readonly Analyzer _analyzer;
        private readonly AstBuilder _builder;
        private readonly AstSolver _solver;
        private readonly Tokenizer _tokenizer;

        public Interpreter()
        {
            _tokenizer = new Tokenizer();
            _analyzer = new Analyzer();
            _builder = new AstBuilder();
            _solver = new AstSolver(_analyzer);
            

        }


        public string Parse(string input)
        {
            var tokens = _tokenizer.GetTokens(input);
            var result = string.Empty;
            if (tokens.Length < 3)
                return "Input some data";

            if (IsExpresion(tokens))
            {
                result = _solver.Solve(_builder.Build(tokens)).ToString();
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
