using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task2.Tests
{
    [TestClass]
    public class MyMathUnitTest
    {
        [TestMethod]
        public void Factorial_PositiveLittleNumber_Calculated()
        {
            var testNumber = 4;
            Assert.AreEqual(24, MyMath.Factorial(testNumber));
        }

        [TestMethod]
        public void Factorial_PositiveHugeNumber_Calculated()
        {
            var testNumber = 10;
            Assert.AreEqual(3628800, MyMath.Factorial(testNumber));
        }

        [TestMethod]
        public void Factorial_NegativeNumber_ThrowException()
        {
            var testNumber = -4;
            try
            {
                MyMath.Factorial(testNumber);
                Assert.Fail("ArgumentOutOfRange exception expected");
            }
            catch (Exception e)
            {
            }
        }

        [TestMethod]
        public void Factorial_VaryBigNumber_Calculated()
        {
            MyMath.Factorial(10000);
        }

        [TestMethod]
        public void Fibonachi_ForTwo_EqualThree()
        {
            Assert.AreEqual(1, MyMath.Fibonachi(2));
        }

        [TestMethod]
        public void Fibonachi_ForLargeNumber_Calculated()
        {
            Assert.AreEqual(144, MyMath.Fibonachi(12));
        }

        [TestMethod]
        public void FibonacciTail_ForTwo_EqualThree()
        {
            Assert.AreEqual((ulong) 1, MyMath.FibonacciTail(2));
        }

        [TestMethod]
        public void FibonacciTail_ForLargeNumber_Calculated()
        {
            Assert.AreEqual((ulong) 144, MyMath.FibonacciTail(12));
        }
    }
}