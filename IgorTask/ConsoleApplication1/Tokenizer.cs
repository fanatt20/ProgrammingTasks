using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public class Tokenizer
    {
        public string[] GetTokens(string input)
        {
            return input.Split(' ');
        }
    }
}
