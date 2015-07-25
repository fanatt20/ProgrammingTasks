using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    public class AstBuilder
    {
        Tokenizer tokenizer;
        private string[] _operatorsArray = { "+", "-", "/", "*" };
        private string[] _braces = { "(", ")" };
        public AstBuilder(Tokenizer tokenizer)
        {

            this.tokenizer = tokenizer;
        }

        public Ast Build(string input)
        {
            Ast head;
            input = input + " )";
            var tokenColl = tokenizer.GetTokens(input);
            head = new Ast(tokenColl[0], isOperator: true);
            int index = 1;
            RecursionBuild(tokenColl, ref index, head);
            return head;
        }

        void RecursionBuild(string[] tokens, ref int index, Ast head)
        {
            while (tokens[index] != ")")
                if (tokens[index] == "(")
                {
                    index++;
                    var newHead = new Ast(tokens[index], isOperator:true);
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
    }
}
