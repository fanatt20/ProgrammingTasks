using System;

namespace Task3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Interpreter interpreter=new Interpreter();

            while (true)
            {
                Console.WriteLine(interpreter.Parse(Console.ReadLine()));
            }
        }
    }
}