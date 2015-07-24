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
            //ExpressionParser parser = new ExpressionParser();
            //while (true)
            //{
            //    Console.WriteLine(parser.Parse(Console.ReadLine()));
                
            //}
            
            AstBuilder builder=new AstBuilder(new Tokenizer());
            Ast tree ;
            AstSolver solver=new AstSolver();

            while (true)
            {
                tree = builder.Build(Console.ReadLine());
                Console.WriteLine(solver.Solve(tree));
            }
            Console.WriteLine();
            Console.ReadKey();

        }
    }
}
