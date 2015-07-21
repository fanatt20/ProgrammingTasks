using System;
using System.Collections.Generic;

namespace Task2
{
    public class MyMath
    {
        private static Dictionary<int, ulong> _fibonacciDict = new Dictionary<int, ulong>{{1,1},{2,1}};

        private static void Main(string[] args)
        {
            Console.ReadKey();
        }

        public static int Factorial(int num)
        {
            return Fact(num, 1);
        }

        private static int Fact(int num, int accamulator)
        {
            if (num == 0)
                return accamulator;
            if (num < 0)
                throw new ArgumentOutOfRangeException("Number must be positive");
            return Fact(num - 1, accamulator * num);
        }

        // Refactor using tail recursion
        public static int Fibonachi(int n)
        {
            if (n == 1)
                return 1;
            if (n == 2)
                return 1;
            return Fibonachi(n - 1) + Fibonachi(n - 2);
        }

        private static ulong FibonacciTail(int num, ulong accum1)
        {
            if (num < 0)
                throw new ArgumentOutOfRangeException("Number must be positive");
            return _fibonacciDict.ContainsKey(num) ? _fibonacciDict[num] : FibonacciTail(num - 1, accum1);
        }

        #region Tests

        #endregion
    }
}

//                 5
//         4                3
//   3         2        2        1
//2     1    1    1  1     1


//Pattern: 
//    Strategy
//    State (Memento)
//    Command
//    Factory Method

//Tests for domino

// 2 + ( 5 * 10 - 30 / 15) *  4 

//lg sin
//var x = 5     
//var y = (x) => x*2

//y(2)

//var y = (x) => y(x-1)*x

//y(3)