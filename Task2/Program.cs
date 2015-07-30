using System;
using System.Collections.Generic;

namespace Task2
{
    public class MyMath
    {
        private static Dictionary<int, int> _fibonacciDict = new Dictionary<int, int>{{1,1},{2,1}};

        private static void Main(string[] args)
        {
            Console.WriteLine();
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

        public static UInt64 FibonacciTail(int num)
        {
            return FibonacciTail(num, 1, 1, 0);
        }
        private static UInt64 FibonacciTail(int num, UInt64 growNum1, UInt64 growNum2 = 1, UInt64 buff = 0)
        {
            if (num-- < 3)
            {
                return growNum1;
            }
            buff = growNum1;
            growNum1 += growNum2;
            growNum2 = buff;
            
            
            return FibonacciTail(num,growNum1,growNum2,buff);

        }
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