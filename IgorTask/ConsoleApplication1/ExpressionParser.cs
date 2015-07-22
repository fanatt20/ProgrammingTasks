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


        public ExpressionParser(string input)
        {
            _input = input;

        }
        public void Parse()
        {
            var regExpr = new Regex(@"\u0028[\u002B|\u002D]?\d+([\u002A|\u002B|\u002D|\u002F]\d+)+\u0029");
            var input = _input;
            while (regExpr.IsMatch(input))
            {

                foreach (var item in regExpr.Matches(input).Cast<Match>())
                {
                    input = input.Replace(item.Value, Calculate(item.Value).ToString());
                }
            }
            var secondRegExpr = new Regex(@"[\u002B|\u002D]?\d+([\u002A|\u002B|\u002D|\u002F]\d+)+");
            while (secondRegExpr.IsMatch(input))
            {

                foreach (var item in secondRegExpr.Matches(input).Cast<Match>())
                {
                    input = input.Replace(item.Value, Calculate(item.Value).ToString());
                }
            }
            Console.WriteLine(input);
        }

        private double Calculate(string value)
        {
            var regExpr = new Regex(@"[\u002D]?\d+[\u002A|\u002B|\u002D|\u002F]\d+");
            var priorityRegExpr= new Regex(@"[\u002D]?\d+[\u002A|\u002F]\d+");
            while (priorityRegExpr.IsMatch(value))
            {

                var firstNumberOperand = string.Empty;
                foreach (var item in priorityRegExpr.Matches(value).Cast<Match>())
                {
                    if (char.IsDigit(item.Value[0]))
                        firstNumberOperand = "+";
                    else
                        firstNumberOperand = string.Empty;
                    value = value.Replace(item.Value, SimpleCalculate(firstNumberOperand + item.Value).ToString());
                }
            }
            while (regExpr.IsMatch(value))
            {

                var firstNumberOperand = string.Empty;
                foreach (var item in regExpr.Matches(value).Cast<Match>())
                {
                    if (char.IsDigit(item.Value[0]))
                        firstNumberOperand = "+";
                    else
                        firstNumberOperand = string.Empty;
                    value = value.Replace(item.Value, SimpleCalculate(firstNumberOperand +item.Value).ToString());
                }
            }

            return double.Parse(value.Replace('(',' ').Replace(')',' '));
        }

        private double SimpleCalculate(string value)
        {
            var leftOperand = new StringBuilder();
            var rightOperand = new StringBuilder();
            var operand = '\0';
            var IsOnleftOperand = false;
            var leftSign = true;
            foreach (var ch in value.ToCharArray())
            {
                if (_symbols.Contains(ch) || leftSign)
                {
                    operand = ch;
                    IsOnleftOperand = !IsOnleftOperand;
                    if (leftSign && (Symbols)ch == Symbols.Minus)
                    {
                        leftSign = false;
                        leftOperand.Append(ch);
                    }
                    leftSign = false;
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
    }
}
