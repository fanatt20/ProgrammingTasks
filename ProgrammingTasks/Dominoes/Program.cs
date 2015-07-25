using System;

namespace Dominoes
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var initChain =
                "13, 35, 54, 43, 32, 24, 46, 61, 11, 13, 35, 51, 11, 16, 61, 11, 14, 43, 36, 66, 61, 15, 51, 14, 43, 31, 16, 64, 42, 26, 65, 52, 23, 35, 51, 16, 66, 62, 21, 13, 32, 26, 65, 56, 63, 34, 44, 45, 56, 66, 64";
            var dominoBuilder = new DominoBuilder(initChain);

            dominoBuilder.Sort();
            Console.WriteLine("--------------------------------------------------------------\n" + dominoBuilder);

            Console.ReadKey();
        }
    }
}