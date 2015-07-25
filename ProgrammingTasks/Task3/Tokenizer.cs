using System.Collections.Generic;
using System.Linq;

namespace Task3
{
    public class Tokenizer
    {
        public string[] GetTokens(string input)
        {
            input = input.Replace("(", " ( ").Replace(")", " ) ").Replace("  ", " ").Replace("  "," ").Trim();
            return input.Split(' ');
        }
    }
}