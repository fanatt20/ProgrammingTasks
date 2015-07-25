namespace Task3
{
    public class AstBuilder
    {
        private readonly Tokenizer _tokenizer;
        private string[] _braces = {"(", ")"};
        private string[] _operatorsArray = {"+", "-", "/", "*"};

        public AstBuilder(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public Ast Build(string input)
        {
            Ast head;
            input = input + " )";
            var tokenColl = _tokenizer.GetTokens(input);
            head = new Ast(tokenColl[0], true);
            var index = 1;
            RecursionBuild(tokenColl, ref index, head);
            return head;
        }

        private void RecursionBuild(string[] tokens, ref int index, Ast head)
        {
            while (tokens[index] != ")")
                if (tokens[index] == "(")
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
    }
}