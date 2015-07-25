using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominoes
{
    internal class Program
    {
        private static bool QuickCheck(List<DominoNode> list)
        {
            var query1 = list.Select(dmn => dmn.FirstNumber).Distinct().ToList();
            var query2 = list.Select(dmn => dmn.SecondNumber).Distinct().ToList();
            if (query1.Count() != query2.Count())
                return false;
            query2.Sort();
            query1.Sort();
            if (query1.Where((el, i) => el != query2[i]).Count() != 0)
                return false;


            var dict = query1.ToDictionary(i => i, i => false);
            foreach (var dominoNode in list)
            {
                dict[dominoNode.FirstNumber] = !dict[dominoNode.FirstNumber];
                dict[dominoNode.SecondNumber] = !dict[dominoNode.SecondNumber];
            }
            return !dict.Any(b => b.Value);
        }

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