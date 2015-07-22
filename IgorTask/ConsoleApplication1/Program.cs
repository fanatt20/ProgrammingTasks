using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Text.RegularExpressions;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ExpressionParser parser = new ExpressionParser(Console.ReadLine());
                parser.Parse();
            }
            
            Console.ReadKey();

        }
    }
}
