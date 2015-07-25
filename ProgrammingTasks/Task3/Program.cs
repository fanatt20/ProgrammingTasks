using System;

namespace Task3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //ExpressionParser parser = new ExpressionParser();
            //while (true)
            //{
            //    Console.WriteLine(parser.Parse(Console.ReadLine()));

            //}

            var builder = new AstBuilder(new Tokenizer());
            Ast tree;
            var solver = new AstSolver();

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