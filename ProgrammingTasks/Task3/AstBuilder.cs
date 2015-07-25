using System;
using System.Data;

namespace Task3
{
    public class AstBuilder
    {
        public AstBuilder()
        {
        }

        public Ast Build(string[] input)
        {
            Ast head;

            head = new Ast(input[1], true);
            var index = 2;
            try
            {
                RecursionBuild(input, ref index, head);
            }
            catch (IndexOutOfRangeException)
            {
                throw new SyntaxErrorException();
            }
            return head;
        }

        private void RecursionBuild(string[] tokens, ref int index, Ast head)
        {
            while (!EndOfExpresion(tokens[index]))
                if (SubExpresion(tokens[index]))
                {
                    index++;
                    var newHead = new Ast(tokens[index], true);
                    index++;
                    RecursionBuild(tokens, ref index, newHead);
                    head.Operands.Add(newHead);
                }
                else
                {
                    head.Operands.Add(new Ast(tokens[index++]));
                }
            index++;
        }

        private static bool EndOfExpresion(string token)
        {
            return token.Contains(")");
        }

        private static bool SubExpresion(string token)
        {
            return token.Contains("(");
        }
    }
}