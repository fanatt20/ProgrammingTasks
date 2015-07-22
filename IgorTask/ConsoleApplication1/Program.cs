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
            ExpressionParser parser = new ExpressionParser();
            while (true)
            {
                Console.WriteLine(parser.Parse(Console.ReadLine()));
                
            }
            
            Console.ReadKey();

        }
    }
}
