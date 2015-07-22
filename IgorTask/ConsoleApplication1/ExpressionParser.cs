using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace Task3
{
    class ExpressionParser
    {
        private string _input;
        char[] _symbols = new char[] { '+', '-', '*', '/' };
        enum Symbols
        {
            Plus = '+',
            Minus = '-',
            Divide = '/',
            Multiply = '*'

        }
        /* Few Unicode Symbols:
            \u0028 <=> (
            \u0029 <=> )
            \u002D <=> -
            \u002B <=> +
            \u002A <=> *
            \u002F <=> /         
         */
        Regex _exprWithBraces = new Regex(@"\u0028[\u002B|\u002D]?\d+([\u002A|\u002B|\u002D|\u002F][\u002D]?\d+)+\u0029");
        Regex _exprWithoutBracers = new Regex(@"[\u002B|\u002D]?\d+([\u002A|\u002B|\u002D|\u002F][\u002D]?\d+)+");
        Regex _minusMinusChecker = new Regex(@"\u002D\u002D");
        Regex _plusAfterSymbolChecker = new Regex(@"[\u002A|\u002B|\u002D|\u002F]\u002B+");
        Regex _plusAndMinusChecker = new Regex(@"[\u002D]?\d+[\u002B|\u002D][\u002D]?\d+");
        Regex _multiplyAndDivideChecker = new Regex(@"[\u002D]?\d+[\u002A|\u002F][\u002D]?\d+");
        Regex _findNumbersWithMinus = new Regex(@"\u002D?\d+");


        public ExpressionParser(string input)
        {
            _input = input;

        }
        public void Parse()
        {

            var input = _input;

            input = _minusMinusChecker.Replace(input, "+");
            while (_plusAfterSymbolChecker.IsMatch(input))
            {
                var symbol = _plusAfterSymbolChecker.Match(input).Value[0].ToString();
                input = _plusAfterSymbolChecker.Replace(input, symbol);
            }
            while (_exprWithBraces.IsMatch(input))
            {

                foreach (var item in _exprWithBraces.Matches(input).Cast<Match>())
                {
                    input = input.Replace(item.Value, Calculate(item.Value).ToString());
                }
            }

            while (_exprWithoutBracers.IsMatch(input))
            {

                foreach (var item in _exprWithoutBracers.Matches(input).Cast<Match>())
                {
                    input = input.Replace(item.Value, Calculate(item.Value).ToString());
                }
            }
            input = _minusMinusChecker.Replace(input, "+");
            Console.WriteLine(input);
        }

        private double Calculate(string value)
        {

            value = _minusMinusChecker.Replace(value, "+");
            while (_multiplyAndDivideChecker.IsMatch(value))
            {

                var firstNumberOperand = string.Empty;
                foreach (var item in _multiplyAndDivideChecker.Matches(value).Cast<Match>())
                {
                    if (char.IsDigit(item.Value[0]))
                        firstNumberOperand = "+";
                    else
                        firstNumberOperand = string.Empty;
                    value = value.Replace(item.Value, TwoNumbersCalculate(firstNumberOperand + item.Value).ToString());
                }
            }
            while (_plusAndMinusChecker.IsMatch(value))
            {

                var firstNumberOperand = string.Empty;
                foreach (var item in _plusAndMinusChecker.Matches(value).Cast<Match>())
                {
                    if (char.IsDigit(item.Value[0]))
                        firstNumberOperand = "+";
                    else
                        firstNumberOperand = string.Empty;
                    value = value.Replace(item.Value, TwoNumbersCalculate(firstNumberOperand + item.Value).ToString());
                }
            }
            return double.Parse(value.Replace('(', ' ').Replace(')', ' '));
        }

        private object TwoNumbersCalculate(string value)
        {
            var leftOperand = string.Empty;
            var rightOperand = string.Empty;
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
                    result = double.Parse(leftOperand.ToString()) / double.Parse(rightOperand.ToString());
                    break;
                case Symbols.Minus:
                    result = double.Parse(leftOperand.ToString()) - double.Parse(rightOperand.ToString());
                    break;
                case Symbols.Multiply:
                    result = double.Parse(leftOperand.ToString()) * double.Parse(rightOperand.ToString());
                    break;
                case Symbols.Plus:
                    result = double.Parse(leftOperand.ToString()) + double.Parse(rightOperand.ToString());
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return result;
        }

        private double PlusAndMinusCalculate(string value)
        {
            var leftOperand = new StringBuilder();
            var rightOperand = new StringBuilder();
            var operand = '\0';
            var IsOnleftOperand = false;
            var leftSignSelected = false;
            if (leftSignSelected && (Symbols)value[0] == Symbols.Minus)
            {
                leftOperand.Append(value[0]);
            }
            leftSignSelected = true;
            foreach (var ch in value.ToCharArray())
            {
                if (leftSignSelected && _symbols.Contains(ch))
                {
                    operand = ch;
                    IsOnleftOperand = !IsOnleftOperand;
                }
                else
                if (IsOnleftOperand)
                    leftOperand.Append(ch);
                else
                    rightOperand.Append(ch);
            }
            var result = 0.0;
            switch ((Symbols)operand)
            {
                case Symbols.Minus:
                    result = double.Parse(leftOperand.ToString()) - double.Parse(rightOperand.ToString());
                    break;
                case Symbols.Plus:
                    result = double.Parse(leftOperand.ToString()) + double.Parse(rightOperand.ToString());
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return result;
        }
    }
}
