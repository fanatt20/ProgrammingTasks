using System.Collections.Generic;
using System.Linq;

namespace Task3
{
    public class Tokenizer
    {
        public string[] GetTokens(string input)
        {
            
            input = input.Replace("(", " ( ").Replace(")", " ) ").Replace("  ", " ").Replace("  "," ").Trim();
            if (input[0] != '(')
                input = "( " + input;
            if (input[input.Length - 1] != ')')
                input = input + " )";
            return input.Split(' ');
        }
    }
}