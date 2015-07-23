using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Task3
{
    internal class ExpressionParser
    {
        /* Few Unicode Symbols:
         *   \u0028 <=> (
         *   \u0029 <=> )
         *   \u002D <=> -
         *   \u002B <=> +
         *   \u002A <=> *
         *   \u002F <=> /   
         *   \u002E <=> .
         *   \u002C <=> ,
         *   \u003D <=> =
         */
        private const string _uMinus = @"\002D";
        private const string _uPlus = @"\u002B";
        private const string _uMultiply = @"\u002A";
        private const string _uDivide = @"\u002F";
        private const string _uLeftBrace = @"\u0028";
        private const string _uRightBrace = @"\u0029";

        private readonly Dictionary<string, Func<double, double>> _funcDictionary = new Dictionary
            <string, Func<double, double>>
        {
            {"SIN", Math.Sin},
            {"COS", Math.Cos},
            {"TAN", Math.Tan},
            {"EXP", Math.Exp}
        };

        private readonly Dictionary<string, double> _variablesDictionary = new Dictionary<string, double>();

        private readonly Regex _exprWithBraces =
            new Regex(
                @"\u0028[\u002B|\u002D]?\d+(\u002E\d+)?([\u002A|\u002B|\u002D|\u002F][\u002D]?\d+(\u002E\d+)?)+\u0029");

        private readonly Regex _exprWithoutBracers =
            new Regex(@"[\u002B|\u002D]?\d+(\u002E\d+)?([\u002A|\u002B|\u002D|\u002F][\u002D]?\d+(\u002E\d+)?)+");

        private readonly Regex _findNumbersWithMinus = new Regex(@"\u002D?\d+(\u002E\d+)?");
        private readonly Regex _funcWithArgument = new Regex(@"\w+\u0028\d+(\u002E\d+)?\u0029");
        private readonly Regex _minusMinusChecker = new Regex(@"\u002D\u002D");

        private readonly Regex _variable =
            new Regex(@"(\u0028[A-z]\w*\u0029)|([\u002A|\u002B|\u002D|\u002F]?[\u002D]?[A-z]\w*[^\u0028][\u002A|\u002B|\u002D|\u002F]?)");
        private readonly Regex _variableWithBraces = new Regex(@"\u0028[A-z]\w*\u0029");
        private readonly Regex _variableWithoutBraces = new Regex(@"[\u002A|\u002B|\u002D|\u002F][\u002D][A-z]\w*[^\u0028]");

        private readonly Regex _declareVariable = new Regex(@"[A-z]\w*\u003D[^\u0028]");
        private readonly Regex _declareFunc = new Regex(@"[A-z]\w*\u003D\u0028");

        private readonly Regex _multiplyAndDivideChecker =
            new Regex(@"[\u002D]?\d+(\u002E\d+)?[\u002A|\u002F][\u002D]?\d+(\u002E\d+)?");

        private readonly Regex _number = new Regex(@"\u002D?\d+(\u002E\d+)?");
        private readonly Regex _plusAfterSymbolChecker = new Regex(@"[\u002A|\u002B|\u002D|\u002F]\u002B+");

        private readonly Regex _plusAndMinusChecker =
            new Regex(@"[\u002D]?\d+(\u002E\d+)?[\u002B|\u002D][\u002D]?\d+(\u002E\d+)?");

        private readonly char[] _symbols = { '+', '-', '*', '/' };
        private readonly Regex _word = new Regex(@"[A-z]\w*");

        public string Parse(string input)
        {
            if (_declareFunc.IsMatch(input))
            {
                DeclareFunc(input);
                input = "Created";
            }
            else if (_declareVariable.IsMatch(input))
            {
                DeclareVariable(input);
                input = "Created";
            }
            else
                input = CalculateExpression(input);

            return input;
        }

        private void DeclareFunc(string input)
        {
            throw new NotImplementedException();
        }

        private void DeclareVariable(string input)
        {
            var variableName = _word.Match(input).Value;
            var variableValue = double.Parse(_number.Match(input).Value);
            if (!_variablesDictionary.ContainsKey(variableName))
            {
                _variablesDictionary.Add(variableName, variableValue);
            }
            else
            {
                _variablesDictionary[variableName] = variableValue;
            }
        }

        private string CalculateExpression(string input)
        {
            input = _minusMinusChecker.Replace(input, "+");
            while (_plusAfterSymbolChecker.IsMatch(input))
            {
                var symbol = _plusAfterSymbolChecker.Match(input).Value[0].ToString();
                input = _plusAfterSymbolChecker.Replace(input, symbol);
            }
            while (_variable.IsMatch(input))
            {
                var item = _variable.Match(input);
                var index = input.IndexOf(item.Value);
                var varName = _word.Match(item.Value).Value;
                var indexInExpr = item.Value.IndexOf(varName);

                input = input.Remove(index + indexInExpr, varName.Length)
                    .Insert(index + indexInExpr, _variablesDictionary[varName].ToString());



            }
            while (_exprWithBraces.IsMatch(input) || _funcWithArgument.IsMatch(input) ||
                   _exprWithoutBracers.IsMatch(input))
            {
                while (_exprWithBraces.IsMatch(input))
                {
                    foreach (var item in _exprWithBraces.Matches(input).Cast<Match>())
                    {
                        var index = input.IndexOf(item.Value);
                        input = input.Remove(index + 1, item.Value.Length - 2)
                            .Insert(index + 1, Calculate(item.Value).ToString());
                    }
                }
                while (_funcWithArgument.IsMatch(input))
                {
                    foreach (var item in _funcWithArgument.Matches(input).Cast<Match>())
                    {
                        var index = input.IndexOf(item.Value);
                        input = input.Remove(index, item.Value.Length)
                            .Insert(index, CalculateFunc(item.Value).ToString());
                    }
                }

                while (_exprWithoutBracers.IsMatch(input))
                {
                    foreach (var item in _exprWithoutBracers.Matches(input).Cast<Match>())
                    {
                        var index = input.IndexOf(item.Value);
                        input = input.Remove(index, item.Value.Length).Insert(index, Calculate(item.Value).ToString());
                    }
                }
            }
            input = _minusMinusChecker.Replace(input, "+");
            return input;
        }
        private double CalculateFunc(string value)
        {
            if (_funcDictionary.ContainsKey(_word.Match(value).Value.ToUpper()))
            {
                return _funcDictionary[_word.Match(value).Value.ToUpper()].Invoke(double.Parse(_number.Match(value).Value));
            }
            return +0;
        }

        private double Calculate(string value)
        {
            value = _minusMinusChecker.Replace(value, "+");
            while (_multiplyAndDivideChecker.IsMatch(value))
            {
                string firstNumberSign;
                foreach (var item in _multiplyAndDivideChecker.Matches(value).Cast<Match>())
                {
                    firstNumberSign = char.IsDigit(item.Value[0]) ? "+" : string.Empty;
                    var index = value.IndexOf(item.Value);
                    value = value.Remove(index, item.Value.Length)
                        .Insert(index, TwoNumbersCalculate(firstNumberSign + item.Value).ToString());
                }
            }
            while (_plusAndMinusChecker.IsMatch(value))
            {
                string firstNumberSign;
                foreach (var item in _plusAndMinusChecker.Matches(value).Cast<Match>())
                {
                    firstNumberSign = char.IsDigit(item.Value[0]) ? "+" : string.Empty;
                    var index = value.IndexOf(item.Value);
                    value = value.Remove(index, item.Value.Length)
                        .Insert(index, TwoNumbersCalculate(firstNumberSign + item.Value).ToString());
                }
            }
            return double.Parse(value.Replace('(', ' ').Replace(')', ' '));
        }

        private object TwoNumbersCalculate(string value)
        {
            string leftOperand;
            string rightOperand;
            var operand = '\0';


            if ((Symbols)value[0] == Symbols.Plus)
                value = value.Remove(0, 1);
            leftOperand = _findNumbersWithMinus.Match(value).Value;
            value = value.Remove(0, leftOperand.Length);

            rightOperand = _findNumbersWithMinus.Match(value).Value;
            if (value.Length == rightOperand.Length)
            {
                value = value.Remove(0, rightOperand.Length);
                operand = '+';
            }
            else
            {
                value = value.Remove(1, rightOperand.Length);
                operand = value[0];
            }


            var result = 0.0;
            switch ((Symbols)operand)
            {
                case Symbols.Divide:
                    result = double.Parse(leftOperand) / double.Parse(rightOperand);
                    break;
                case Symbols.Minus:
                    result = double.Parse(leftOperand) - double.Parse(rightOperand);
                    break;
                case Symbols.Multiply:
                    result = double.Parse(leftOperand) * double.Parse(rightOperand);
                    break;
                case Symbols.Plus:
                    result = double.Parse(leftOperand) + double.Parse(rightOperand);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return result;
        }

        private enum Symbols
        {
            Plus = '+',
            Minus = '-',
            Divide = '/',
            Multiply = '*'
        }
    }
}