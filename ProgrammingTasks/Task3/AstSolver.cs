using System;
using System.Collections.Generic;

namespace Task3
{
    public class AstSolver
    {
        private readonly Dictionary<string, Func<double, double, double>> _binarFuncDictionary = new Dictionary
            <string, Func<double, double, double>>
        {
            {"+", (a, b) => a + b},
            {"-", (a, b) => a - b},
            {"/", (a, b) => a/b},
            {"*", (a, b) => a*b}
        };

        private readonly Dictionary<string, Func<double, double>> _unarFuncDictionary = new Dictionary
            <string, Func<double, double>>
        {
            {"SIN", Math.Sin},
            {"COS", Math.Cos},
            {"TAN", Math.Tan},
            {"EXP", Math.Exp}
        };
        private Analyzer _analyzer;

        public AstSolver(Analyzer _analyzer)
        {
            this._analyzer = _analyzer;
        }

        public double Solve(Ast node)
        {
            var possibleResult = 0.0;
            _analyzer.ReplaceVaribales(node);
            return double.TryParse(node.Value,out possibleResult)?possibleResult: GetValue(node);
        }

        private double ApplyOperation(string p, List<Ast> list)
        {
            double result = 0;
            if (_binarFuncDictionary.ContainsKey(p))
            {
                result = GetValue(list[0]);
                for (var i = 1; i < list.Count; i++)
                {
                    result = _binarFuncDictionary[p](result, GetValue(list[i]));
                }
            }
            return result;
        }

        private double GetValue(Ast node)
        {
            if (!node.IsOperator)
                return double.Parse(node.Value);
            return ApplyOperation(node.Value, node.Operands);
        }
    }
}